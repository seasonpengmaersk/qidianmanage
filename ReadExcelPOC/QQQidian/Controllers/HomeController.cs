using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QQQidian.Models;
using ReadExcelPOC.Models;
using ReadExcelPOC.Util.Common;

namespace QQQidian.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> log_;

        public HomeController(ILogger<HomeController> logger)
        {
            log_ = logger;
        }


        public IActionResult Index()
        {
            ViewBag.defaultDT = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> getStatistics([FromForm]string queryDatetime)
        {
            log_.LogInformation("Enter getStatistics");
            ResponseObject ro = new ResponseObject();
            ro.Result = "OK";


            if (String.IsNullOrEmpty(queryDatetime))
            {
                ro.Result = "Error";
                ro.ErrorMessage = "Datetime is null or empty";
                return Ok(ro);
            }

            DateTime dt = DateTime.Now;
            if (!DateTime.TryParseExact(queryDatetime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                ro.Result = "Error";
                ro.ErrorMessage = "Invalid Datetime:" + queryDatetime;
                return Ok(ro);
            }

            queryDatetime = queryDatetime.Replace("-", "");

            try
            {
                string url = "https://api.qidian.qq.com/cgi-bin/token?grant_type=client_credential&appid=202010648&secret=GRO7brrdHhtrb9Te";
                string tokenJson = await HttpHelper.HttpGetAsync(url, null);
                Dictionary<string, string> tokenDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenJson);
                string token = tokenDic["access_token"];

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                    return Ok(ro);
                }

                string tokenurl = "https://api.qidian.qq.com/cgi-bin/call/KA/sessionReport?access_token=" + token;
                ReqBody reqBody = new ReqBody();
                reqBody.date = queryDatetime;


                string ret = await HttpHelper.HttpPostAsync(tokenurl, JsonConvert.SerializeObject(reqBody), "application/json", 30, null);

                //Dictionary<string, string> retDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(ret);

                var definition = new { Code = "", ErrCode = "" };
                var retJsonObject = JsonConvert.DeserializeAnonymousType(ret, definition);

                //string code = retDic["code"];
                if (retJsonObject == null || retJsonObject.Code == null || !retJsonObject.Code.Equals("0"))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "server return error.";
                    ro.ResultObject = ret;
                    log_.LogWarning("server return error. The return result is " + ret);
                    return Ok(ro);
                }

                ro.ResultObject = ret;


                string email = "jiarong_xie@outlook.com";
                using (var client = new HttpClient())
                {
                    string jsoncsvurl = "https://json-csv.com/api/getcsv";
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("email", email),
                        new KeyValuePair<string, string>("json", ret)
                    });
                    var result = client.PostAsync(jsoncsvurl, content).Result;
                    string csvContent = result.Content.ReadAsStringAsync().Result;
                    System.Text.Encoding encoding = new System.Text.UTF8Encoding(true);
                    byte[] byteArray = encoding.GetBytes(csvContent);
                    string fileName = String.Format("json_{0}.csv", queryDatetime);
                    fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
                    var data = Encoding.UTF8.GetPreamble().Concat(byteArray).ToArray();

                    log_.LogInformation("End getStatistics");
                    return File(data, "application/csv", fileName);
                }

            }
            catch (Exception e)
            {
                log_.LogError("Exception occur", e);
                ro.Result = "Error";
                ro.ErrorMessage = "Fail to get token.Due to" + e.Message;
                return Ok(ro);
            }


        }

        [HttpGet("cus")]
        public async Task<IActionResult> getCustomerInfos()
        {
            log_.LogInformation("Enter getCustomerInfos");
            ResponseObject ro = new ResponseObject();
            try
            {
                string url = "https://api.qidian.qq.com/cgi-bin/token?grant_type=client_credential&appid=202010648&secret=GRO7brrdHhtrb9Te";
                string tokenJson = await HttpHelper.HttpGetAsync(url, null);
                Dictionary<string, string> tokenDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenJson);
                string token = tokenDic["access_token"];

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                }
                else
                {
                    //get All cusomterId 
                    url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getCustList?next_custid=&access_token=" + token;
                    string customersJson = await HttpHelper.HttpGetAsync(url, null);
                    var CustomersResultDefinition = new { total = "", count = "", data = new { cust_id = new List<string>() }, next_custid = "" };
                    var retJsonObject = JsonConvert.DeserializeAnonymousType(customersJson, CustomersResultDefinition);

                    if (retJsonObject == null || retJsonObject.data == null || retJsonObject.data.cust_id == null)
                    {
                        ro.Result = "Error";
                        ro.ErrorMessage = "CustomersResultDefinition == null || CustomersResultDefinition.data || CustomersResultDefinition.data.cust_id == null";
                        ro.ResultObject = retJsonObject;
                        return Ok(ro);
                    }

                    List<string> customerIds = retJsonObject.data.cust_id;

                    ConcurrentQueue<JObject> returnArray = new ConcurrentQueue<JObject>();
                    log_.LogDebug("Before get customers info reccurlly");
                    List<Task<JObject>> tasks = new List<Task<JObject>>();
                    foreach (string cusId in customerIds)
                    {
                        Thread.Sleep(100);
                        tasks.Add(getCustomerRunner(cusId, token, returnArray));
                        //if (retJson.Result != null)
                        //{
                        //    returnArray.Add(retJson.Result);
                        //}
                    }

                    await Task.WhenAll(tasks).ContinueWith(p =>
                     {
                         log_.LogDebug("After get customers info reccurlly");
                     }, TaskContinuationOptions.OnlyOnRanToCompletion);


                    string email = "jiarong_xie@outlook.com";
                    using (var client = new HttpClient())
                    {
                        string jsoncsvurl = "https://json-csv.com/api/getcsv";

                        //超长url 不能用FormUrlEncodedContent
                        //var content = new FormUrlEncodedContent(new[]
                        //{
                        //    new KeyValuePair<string, string>("email", email),
                        //    new KeyValuePair<string, string>("json", JsonConvert.SerializeObject(returnArray))
                        //});

                        var options = new[]
                        {
                                 new KeyValuePair<string, string>("email", email),
                                 new KeyValuePair<string, string>("json", JsonConvert.SerializeObject(returnArray))
                             };



                        var encodedItems = options.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));

                        //处理超长url的workround
                        var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

                        var result = client.PostAsync(jsoncsvurl, encodedContent).Result;
                        string csvContent = result.Content.ReadAsStringAsync().Result;

                        if (csvContent.IndexOf("A problem occured") != -1)
                        {
                            ro.ErrorMessage = csvContent;
                            ro.Result = "Error";
                        }
                        else
                        {
                            System.Text.Encoding encoding = new System.Text.UTF8Encoding(true);
                            byte[] byteArray = encoding.GetBytes(csvContent);
                            string fileName = String.Format("cusInfoJson_{0}.csv", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
                            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
                            var data = Encoding.UTF8.GetPreamble().Concat(byteArray).ToArray();

                            log_.LogInformation("End getCustomerInfos");
                            return File(data, "application/csv", fileName);
                        }
                    }
                }
            }
            catch (TaskCanceledException te)
            {
                ro.ErrorMessage = te.Message;
                ro.Result = "Error";
                log_.LogInformation("TaskCanceledException Occure.", te);
            }
            catch (Exception e)
            {
                ro.ErrorMessage = e.Message;
                ro.Result = "Error";
                log_.LogInformation("Exception Occure.", e);
            }

            log_.LogInformation("Leave getCustomerInfos");
            return Ok(ro);
        }


        private async Task<JObject> getCustomerRunner(string CustomerId, string token, ConcurrentQueue<JObject> queue)
        {
            log_.LogDebug(String.Format("Start getCustomerRunner.CustomerId = {0}", CustomerId));
            string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getSingCustBaseInfo?access_token=" + token;
            JObject jo = new JObject();
            JArray ja = new JArray();
            jo.Add("cust_id", CustomerId);
            ja.Add("identity");
            ja.Add("contact");
            ja.Add("socialAccount");
            ja.Add("controlInfo");

            jo.Add("data", ja);

            int maximunRetryCount = 2;
            JObject jObject = null;

            for (int i = 0; i < maximunRetryCount; i++)
            {
                var resultJson = await HttpHelper.HttpPostAsync(url, JsonConvert.SerializeObject(jo), "application/json", 180, null);

                try
                {
                    jObject = JsonConvert.DeserializeObject<JObject>(resultJson);
                    JToken value = "";
                    if (jObject.TryGetValue("errcode", out value))
                    {
                        log_.LogWarning(String.Format("Fail to get the Info of this Customer.CustomerId = {0}.Retry Count = {1}", CustomerId, i));
                        jObject = new JObject();
                        jObject.Add("cust_id", CustomerId);
                    }
                    else
                    {
                        break;
                    }

                }
                catch (Exception e)
                {
                    log_.LogError("Get CustomerInfo failed.CustomerId=" + CustomerId, e);
                }
            }

            queue.Enqueue(jObject);
            log_.LogDebug(String.Format("End getCustomerRunner.CustomerId = {0}", CustomerId));
            return jObject;
        }
    }
}
