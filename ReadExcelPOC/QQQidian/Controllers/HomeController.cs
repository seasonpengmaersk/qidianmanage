﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using QQQidian.Util;
using System.IO;

namespace QQQidian.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> log_;
        public IHttpHelper httpHelper_ { get; private set; }

        public HomeController(ILogger<HomeController> logger, IHttpHelper helper)
        {
            log_ = logger;
            httpHelper_ = helper;
        }

        private string json_csv_account = "season.peng@gukodigital.com";


        public IActionResult Index()
        {
            ViewBag.defaultDT = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
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
                string token = await getToken();

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                    return Ok(ro);
                }

                string tokenurl = "https://api.qidian.qq.com/cgi-bin/call/KA/sessionReport?access_token=" + token;
                ReqBody reqBody = new ReqBody();
                reqBody.date = queryDatetime;


                string ret = await httpHelper_.HttpPostAsync(tokenurl, JsonConvert.SerializeObject(reqBody), "application/json", 30, null);

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


                string email = json_csv_account;
                using (var client = new HttpClient())
                {
                    string jsoncsvurl = "https://json-csv.com/api/getcsv";
                    var options = new[]
                    {
                                 new KeyValuePair<string, string>("email", email),
                                 new KeyValuePair<string, string>("json", ret)
                             };



                    var encodedItems = options.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));

                    //处理超长url的workround
                    var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

                    var result = client.PostAsync(jsoncsvurl, encodedContent).Result;
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
                log_.LogError(e, "Exception occur", null);
                ro.Result = "Error";
                ro.ErrorMessage = "Fail to get token.Due to" + e.Message;
                return Ok(ro);
            }


        }

        [HttpGet("accInfo")]
        public IActionResult getAccInfos()
        {
            return Ok(json_csv_account);
        }

        public async Task<string> getToken()
        {
            string url = "https://api.qidian.qq.com/cgi-bin/token?grant_type=client_credential&appid=202010648&secret=GRO7brrdHhtrb9Te";
            string tokenJson = await httpHelper_.HttpGetAsync(url, null);
            Dictionary<string, string> tokenDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenJson);
            string token = tokenDic["access_token"];
            return token;
        }

        public static bool flag = false;

        [HttpGet("cusbg")]
        public IActionResult getCustomerInfoBackground()
        {

            if (!flag)
            {
                flag = true;
                Task.Run(() => getCustomerInfoBackgroundExecutor());
                return Ok("后台任务已经启动,请耐心等待结果。");
            }
            else
            {
                return Ok("后台任务已经在运行中,请耐心等待结果。");
            }
        }



        public async Task getCustomerInfoBackgroundExecutor()
        {
            log_.LogInformation("Enter getCustomerInfoExecutor");
            log_.LogInformation("using account is " + json_csv_account);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ResponseObject ro = new ResponseObject();
            try
            {
                if (ifCusInfoCacheExists() != null)
                {
                    return;
                }


                log_.LogInformation("Cache file of today not found.");

                string token = await getToken();

                DateTime dtToken = DateTime.Now;

                log_.LogInformation(String.Format("Token is {0}", token));

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                }
                else
                {
                    //get All cusomterId 
                    List<string> customerIds = new List<string>();
                    List<string> tempIds = new List<string>();
                    string nextCusId = "";
                    int totalCount = 0;
                    do
                    {
                        string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getCustList?next_custid=" + nextCusId + "&access_token=" + token;
                        string customersJson = await httpHelper_.HttpGetAsync(url, null);
                        var CustomersResultDefinition = new { total = "", count = "", data = new { cust_id = new List<string>() }, next_custid = "" };
                        var retJsonObject = JsonConvert.DeserializeAnonymousType(customersJson, CustomersResultDefinition);


                        if (retJsonObject == null || retJsonObject.data == null || retJsonObject.data.cust_id == null)
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "CustomersResultDefinition == null || CustomersResultDefinition.data || CustomersResultDefinition.data.cust_id == null";
                            ro.ResultObject = retJsonObject;
                            log_.LogError(ro.ErrorMessage + "Executor stop.");
                            flag = false;
                            return;
                        }

                        nextCusId = retJsonObject.next_custid;
                        int tempCount = 0;
                        if (!int.TryParse(retJsonObject.count, out tempCount))
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "Total Count is not a integer.";
                            log_.LogError(ro.ErrorMessage + "Executor stop.");
                            flag = false;
                            return;

                        }

                        totalCount = totalCount + tempCount;

                        tempIds = retJsonObject.data.cust_id;
                        customerIds.AddRange(tempIds);
                    } while (!String.IsNullOrEmpty(nextCusId));

                    if (totalCount != customerIds.Count)
                    {
                        ro.Result = "Error";
                        ro.ErrorMessage = "Total Count not tally with the records count in the result object.";
                        log_.LogError(ro.ErrorMessage + "Executor stop.");
                        flag = false;
                        return;
                    }

                    log_.LogInformation("Total Customer Count=" + customerIds.Count);

                    var tasks = new List<Task<JObject>>();
                    ConcurrentQueue<JObject> returnArray = new ConcurrentQueue<JObject>();
                    LimitedConcurrencyLevelTaskScheduler ts = new LimitedConcurrencyLevelTaskScheduler(1);
                    TaskFactory factory = new TaskFactory(ts);
                    log_.LogDebug("Before get customers info reccurlly.");
                    int idx = 0;
                    int limitCount = 500;
                    //customerIds = customerIds.GetRange(0, 100);
                    foreach (string cusId in customerIds)
                    {
                        idx++;
                        log_.LogDebug(String.Format("Start task {0}.CusId = {1}", idx, cusId));
                        tasks.Add(getCustomerRunner(cusId, token));

                        if (idx % limitCount == 0)
                        {
                            log_.LogInformation("到达阈值，现在的idx=" + idx);
                            //等待该批任务全部完成，然后进入下一批

                            foreach (Task<JObject> t in tasks)
                            {
                                var ret = await t;
                                returnArray.Enqueue(ret);
                            }

                            //清空该批已经完成的task
                            tasks.Clear();
                            //休息6秒
                            Thread.Sleep(6000);
                        }
                    }

                    foreach (Task<JObject> t in tasks)
                    {
                        var ret = await t;
                        returnArray.Enqueue(ret);
                    }

                    await Task.WhenAll(tasks).ContinueWith(p =>
                    {
                        log_.LogDebug("After get customers info reccurlly");
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);

                    string email = json_csv_account;

                    string jsoncsvurl = "https://json-csv.com/api/getcsv";

                    var options = new[]
                    {
                                 new KeyValuePair<string, string>("email", email),
                                 new KeyValuePair<string, string>("json", JsonConvert.SerializeObject(returnArray))
                    };



                    var encodedItems = options.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));

                    //处理超长url的workround
                    var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");
                    try
                    {
                        string csvContent = await httpHelper_.HttpPostAsync(jsoncsvurl, encodedContent, null, 600);

                        if (csvContent.IndexOf("A problem occured") != -1)
                        {
                            ro.ErrorMessage = csvContent;
                            ro.Result = "Error";
                            log_.LogInformation("Error Occur when convert to csv." + csvContent);
                        }
                        else
                        {
                            System.Text.Encoding encoding = new System.Text.UTF8Encoding(true);
                            byte[] byteArray = encoding.GetBytes(csvContent);
                            string fileName = String.Format("cusInfoJson_{0}.csv", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
                            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
                            var data = Encoding.UTF8.GetPreamble().Concat(byteArray).ToArray();

                            cacheTheCusInfoFile(data, fileName);

                            log_.LogInformation("End getCustomerInfosBackgroundExecutor");
                            flag = false;
                            return;
                        }
                    }
                    catch (Exception csvE)
                    {
                        log_.LogError(csvE, "encounter exception when convert JSON.", null);
                    }


                }
            }
            catch (TaskCanceledException te)
            {
                ro.ErrorMessage = te.Message;
                ro.Result = "Error";
                log_.LogError(te, "TaskCanceledException Occure.", null);
            }
            catch (Exception e)
            {
                ro.ErrorMessage = e.Message;
                ro.Result = "Error";
                log_.LogError(e, "Exception Occure.", null);
            }
            stopwatch.Stop();
            log_.LogInformation("Leave getCustomerInfosBackgroundExecutor.TimeElaps=" + stopwatch.Elapsed.TotalSeconds);
            flag = false;
            return;
        }

        private FileStreamResult ifCusInfoCacheExists()
        {
            string testFileName = String.Format("cusInfoJson_{0}", string.Format("{0:yyyyMMdd}", DateTime.Now));
            string yesterdayFileName = String.Format("cusInfoJson_{0}", string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(-1)));
            string baseDir = AppContext.BaseDirectory;
            string CustomerInfosDirPath = Path.Combine(baseDir, "CustomerInfos");
            DirectoryInfo CustomerInfosDir = new DirectoryInfo(CustomerInfosDirPath);
            if (!Directory.Exists(CustomerInfosDirPath))
            {
                CustomerInfosDir = Directory.CreateDirectory(CustomerInfosDirPath);
            }

            FileInfo[] files = CustomerInfosDir.GetFiles().Where(f => f.Name.StartsWith(testFileName) && f.Name.EndsWith(".csv")).ToArray();
            FileInfo[] yesterdayFiles = CustomerInfosDir.GetFiles().Where(f => f.Name.StartsWith(yesterdayFileName) && f.Name.EndsWith(".csv")).ToArray();

            //remove yesterday files
            foreach (FileInfo ff in yesterdayFiles)
            {
                ff.Delete();
            }

            if (files != null && files.Length > 0)
            {
                IOrderedEnumerable<FileInfo> ioe = files.OrderByDescending(a => a.LastWriteTimeUtc);
                FileInfo fi = ioe.FirstOrDefault();
                log_.LogInformation("End ifCusInfoCacheExists with cache file");
                return File(fi.OpenRead(), "application/csv", fi.Name);
            }
            else
            {
                return null;
            }
        }

        private FileStreamResult ifOwnerInfoCacheExists()
        {
            string testFileName = String.Format("ownerInfoJson_{0}", string.Format("{0:yyyyMMdd}", DateTime.Now));
            string yesterdayFileName = String.Format("ownerInfoJson_{0}", string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(-1)));
            string baseDir = AppContext.BaseDirectory;
            string OwnerInfosDirPath = Path.Combine(baseDir, "OwnerInfos");
            DirectoryInfo OwnerInfosDir = new DirectoryInfo(OwnerInfosDirPath);
            if (!Directory.Exists(OwnerInfosDirPath))
            {
                OwnerInfosDir = Directory.CreateDirectory(OwnerInfosDirPath);
            }

            FileInfo[] files = OwnerInfosDir.GetFiles().Where(f => f.Name.StartsWith(testFileName) && f.Name.EndsWith(".csv")).ToArray();
            FileInfo[] yesterdayFiles = OwnerInfosDir.GetFiles().Where(f => f.Name.StartsWith(yesterdayFileName) && f.Name.EndsWith(".csv")).ToArray();

            //remove yesterday files
            foreach (FileInfo ff in yesterdayFiles)
            {
                ff.Delete();
            }

            if (files != null && files.Length > 0)
            {
                IOrderedEnumerable<FileInfo> ioe = files.OrderByDescending(a => a.LastWriteTimeUtc);
                FileInfo fi = ioe.FirstOrDefault();
                log_.LogInformation("End ifCusInfoCacheExists with cache file");
                return File(fi.OpenRead(), "application/csv", fi.Name);
            }
            else
            {
                return null;
            }
        }

        private void cacheTheCusInfoFile(byte[] data, string fileName)
        {
            log_.LogInformation("Enter cache cusinfo file" + fileName);
            string baseDir = AppContext.BaseDirectory;
            string CustomerInfosDirPath = Path.Combine(baseDir, "CustomerInfos");
            //cache the file
            FileStream fs = null;
            try
            {
                string fullLocalFileName = Path.Combine(CustomerInfosDirPath, fileName);
                FileInfo localFile = new FileInfo(fullLocalFileName);
                fs = localFile.OpenWrite();
                fs.Write(data, 0, data.Length);
                fs.Flush();

            }
            catch (Exception fe)
            {
                log_.LogWarning("Write cache file encounter error", fe);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            log_.LogInformation("End cache cusinfo file" + fileName);
        }

        private void cacheTheOwnerInfoFile(byte[] data, string fileName)
        {
            log_.LogInformation("Enter cache ownerinfo file" + fileName);
            string baseDir = AppContext.BaseDirectory;
            string ownerInfosDirPath = Path.Combine(baseDir, "OwnerInfos");
            //cache the file
            FileStream fs = null;
            try
            {
                string fullLocalFileName = Path.Combine(ownerInfosDirPath, fileName);
                FileInfo localFile = new FileInfo(fullLocalFileName);
                fs = localFile.OpenWrite();
                fs.Write(data, 0, data.Length);
                fs.Flush();

            }
            catch (Exception fe)
            {
                log_.LogWarning("Write owner cache file encounter error", fe);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            log_.LogInformation("End owner cache cusinfo file" + fileName);
        }

        //GetCusInfoSynchronize
        [HttpGet("cussync")]
        public async Task<IActionResult> getCustomerInfoSync()
        {
            log_.LogInformation("Enter getCustomerInfosync");
            log_.LogInformation("using account is " + json_csv_account);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ResponseObject ro = new ResponseObject();
            try
            {

                var cacheFile = ifCusInfoCacheExists();
                if (cacheFile != null)
                {
                    return cacheFile;
                }

                log_.LogInformation("Cache file of today not found.");

                string token = await getToken();

                DateTime dtToken = DateTime.Now;

                log_.LogInformation(String.Format("Token is {0}", token));

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                }
                else
                {
                    //get All cusomterId 
                    List<string> customerIds = new List<string>();
                    List<string> tempIds = new List<string>();
                    string nextCusId = "";
                    int totalCount = 0;
                    do
                    {
                        string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getCustList?next_custid=" + nextCusId + "&access_token=" + token;
                        string customersJson = await httpHelper_.HttpGetAsync(url, null);
                        var CustomersResultDefinition = new { total = "", count = "", data = new { cust_id = new List<string>() }, next_custid = "" };
                        var retJsonObject = JsonConvert.DeserializeAnonymousType(customersJson, CustomersResultDefinition);


                        if (retJsonObject == null || retJsonObject.data == null || retJsonObject.data.cust_id == null)
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "CustomersResultDefinition == null || CustomersResultDefinition.data || CustomersResultDefinition.data.cust_id == null";
                            ro.ResultObject = retJsonObject;
                            return Ok(ro);
                        }

                        nextCusId = retJsonObject.next_custid;
                        int tempCount = 0;
                        if (!int.TryParse(retJsonObject.count, out tempCount))
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "Total Count is not a integer.";
                            return Ok(ro);
                        }

                        totalCount = totalCount + tempCount;

                        tempIds = retJsonObject.data.cust_id;
                        customerIds.AddRange(tempIds);
                    } while (!String.IsNullOrEmpty(nextCusId));

                    if (totalCount != customerIds.Count)
                    {
                        ro.Result = "Error";
                        ro.ErrorMessage = "Total Count not tally with the records count in the result object.";
                        return Ok(ro);
                    }

                    log_.LogInformation("Total Customer Count=" + customerIds.Count);

                    ConcurrentQueue<JObject> returnArray = new ConcurrentQueue<JObject>();
                    log_.LogDebug("Before get customers info reccurlly.");
                    int idx = 0;
                    foreach (string cusId in customerIds)
                    {
                        idx++;
                        DateTime now = DateTime.Now;
                        TimeSpan ts = now - dtToken;
                        log_.LogDebug(String.Format("Start task {0}.CusId = {1}", idx, cusId));
                        if (ts.TotalSeconds > 5400)
                        {
                            token = await getToken();
                            dtToken = now;
                        }
                        var r = await getCustomerRunner(cusId, token);
                        returnArray.Enqueue(r);
                        //if (retJson.Result != null)
                        //{
                        //    returnArray.Add(retJson.Result);
                        //}
                    }

                    string email = json_csv_account;
                    using (var client = new HttpClient())
                    {
                        string jsoncsvurl = "https://json-csv.com/api/getcsv";

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

                            //cache the file
                            cacheTheCusInfoFile(data, fileName);


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
                log_.LogError(te, "TaskCanceledException Occure.", null);
            }
            catch (Exception e)
            {
                ro.ErrorMessage = e.Message;
                ro.Result = "Error";
                log_.LogError(e, "Exception Occure.", null);
            }
            stopwatch.Stop();
            log_.LogInformation("Leave getCustomerInfos.TimeElaps=" + stopwatch.Elapsed.TotalSeconds);
            return Ok(ro);
        }

        //GetCusInfo
        [HttpGet("cus")]
        public async Task<IActionResult> getCustomerInfosAsync()
        {
            log_.LogInformation("Enter getCustomerInfos");
            log_.LogInformation("using account is " + json_csv_account);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ResponseObject ro = new ResponseObject();
            try
            {
                var cacheFile = ifCusInfoCacheExists();
                if (cacheFile != null)
                {
                    return cacheFile;
                }

                log_.LogInformation("Cache file of today not found.");
                string token = await getToken();

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                }
                else
                {
                    //get All cusomterId 
                    List<string> customerIds = new List<string>();
                    List<string> tempIds = new List<string>();
                    string nextCusId = "";
                    int totalCount = 0;
                    do
                    {
                        string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getCustList?next_custid=" + nextCusId + "&access_token=" + token;
                        string customersJson = await httpHelper_.HttpGetAsync(url, null);
                        var CustomersResultDefinition = new { total = "", count = "", data = new { cust_id = new List<string>() }, next_custid = "" };
                        var retJsonObject = JsonConvert.DeserializeAnonymousType(customersJson, CustomersResultDefinition);


                        if (retJsonObject == null || retJsonObject.data == null || retJsonObject.data.cust_id == null)
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "CustomersResultDefinition == null || CustomersResultDefinition.data || CustomersResultDefinition.data.cust_id == null";
                            ro.ResultObject = retJsonObject;
                            return Ok(ro);
                        }

                        nextCusId = retJsonObject.next_custid;
                        int tempCount = 0;
                        if (!int.TryParse(retJsonObject.count, out tempCount))
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "Total Count is not a integer.";
                            return Ok(ro);
                        }

                        totalCount = totalCount + tempCount;

                        tempIds = retJsonObject.data.cust_id;
                        customerIds.AddRange(tempIds);
                    } while (!String.IsNullOrEmpty(nextCusId));

                    if (totalCount != customerIds.Count)
                    {
                        ro.Result = "Error";
                        ro.ErrorMessage = "Total Count not tally with the records count in the result object.";
                        return Ok(ro);
                    }

                    log_.LogInformation("Total Customer Count=" + customerIds.Count);

                    ConcurrentQueue<JObject> returnArray = new ConcurrentQueue<JObject>();
                    log_.LogDebug("Before get customers info reccurlly.");
                    var tasks = new List<Task>();
                    LimitedConcurrencyLevelTaskScheduler ts = new LimitedConcurrencyLevelTaskScheduler(500);
                    TaskFactory factory = new TaskFactory(ts);
                    int idx = 0;
                    int limitCount = 500;
                    //customerIds = customerIds.GetRange(0, 100);
                    foreach (string cusId in customerIds)
                    {
                        idx++;
                        log_.LogDebug(String.Format("Start task {0}.CusId = {1}", idx, cusId));
                        tasks.Add(getCustomerRunner(cusId, token));

                        if (idx % limitCount == 0)
                        {
                            log_.LogInformation("到达阈值，现在的idx=" + idx);
                            //等待该批任务全部完成，然后进入下一批

                            foreach (Task<JObject> t in tasks)
                            {
                                var ret = await t;
                                returnArray.Enqueue(ret);
                            }

                            //清空该批已经完成的task
                            tasks.Clear();
                            //休息6秒
                            Thread.Sleep(6000);
                        }
                    }

                    foreach (Task<JObject> t in tasks)
                    {
                        var ret = await t;
                        returnArray.Enqueue(ret);
                    }

                    await Task.WhenAll(tasks).ContinueWith(p =>
                    {
                        log_.LogDebug("After get customers info reccurlly");
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);

                    string email = json_csv_account;

                    string jsoncsvurl = "https://json-csv.com/api/getcsv";

                    var options = new[]
                    {
                                 new KeyValuePair<string, string>("email", email),
                                 new KeyValuePair<string, string>("json", JsonConvert.SerializeObject(returnArray))
                             };



                    var encodedItems = options.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));

                    //处理超长url的workround
                    var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

                    string csvContent = await httpHelper_.HttpPostAsync(jsoncsvurl, encodedContent, null, 600);

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

                        //cache the result file
                        cacheTheCusInfoFile(data, fileName);

                        log_.LogInformation("End getCustomerInfos");
                        return File(data, "application/csv", fileName);
                    }

                }
            }
            catch (TaskCanceledException te)
            {
                ro.ErrorMessage = te.Message;
                ro.Result = "Error";
                log_.LogError(te, "TaskCanceledException Occure.", null);
            }
            catch (Exception e)
            {
                ro.ErrorMessage = e.Message;
                ro.Result = "Error";
                log_.LogError(e, "Exception Occure.", null);
            }
            stopwatch.Stop();
            log_.LogInformation("Leave getCustomerInfos.TimeElaps=" + stopwatch.Elapsed.TotalSeconds);
            return Ok(ro);
        }

        //
        [HttpGet("test")]
        public IActionResult Json2Csv(string jsonString)
        {
            string csvContent = "";
            jsonString = @"{
                      'channel': {
                        'title': 'James Newton-King',
                        'link': 'http://james.newtonking.com',
                        'description': 'James Newton-King\'s blog.',
                        'item': [
                          {
                            'title': 'Json.NET 1.3 + New license + Now on CodePlex',
                            'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
                            'link': 'http://james.newtonking.com/projects/json-net.aspx',
                            'categories': [
                              'Json.NET',
                              'CodePlex'
                            ]
                          },
                          {
                            'title': 'LINQ to JSON beta',
                            'description': 'Announcing LINQ to JSON',
                            'link': 'http://james.newtonking.com/projects/json-net.aspx',
                            'categories': [
                              'Json.NET',
                              'LINQ'
                            ]
                          }
                        ]
                      }
                    }";

            JObject rss = JObject.Parse(jsonString);
            DataTable dt = new DataTable();

            JEnumerable<JToken> je = rss.Children();
            IList<string> stringList = new List<string>();
            foreach (JToken jt in je)
            {
                getChildValue(jt, dt);
            }



            System.Text.Encoding encoding = new System.Text.UTF8Encoding(true);
            byte[] byteArray = encoding.GetBytes(csvContent);
            string fileName = String.Format("cusInfoJson_{0}.csv", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
            var data = Encoding.UTF8.GetPreamble().Concat(byteArray).ToArray();

            log_.LogInformation("End getCustomerInfos");
            //return File(data, "application/csv", fileName);

            return Ok(csvContent);
        }

        private void getChildValue(JToken j, DataTable dt)
        {

            JEnumerable<JToken> je = j.Children();
            if (je.Count<JToken>() > 0)
            {
                foreach (JToken jt in je)
                {
                    getChildValue(jt, dt);
                    Console.WriteLine("HasChildren.I am " + jt.Path);
                }
            }
            else
            {
                //retList.Add(j.Path +":" + j.CreateReader().ReadAsString());
                Regex reg = new Regex("\\[[0-9]*\\]");
                String columnName = reg.Replace(j.Path, "");
                if (!dt.Columns.Contains(columnName))
                {
                    dt.Columns.Add(j.Path);
                }
                DataRow dr = dt.NewRow();
                dr[columnName] = j.CreateReader().ReadAsString();

            }

        }

        private async Task<JObject> getCustomerRunner(string CustomerId, string token)
        {
            Random r = new Random();
            int randomSleepTime = r.Next(1, 5);
            //Thread.Sleep(randomSleepTime * 500);
            log_.LogInformation(String.Format("Start getCustomerRunner.CustomerId = {0}", CustomerId));
            string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getSingCustBaseInfo?access_token=" + token;
            JObject jo = new JObject();
            JArray ja = new JArray();
            jo.Add("cust_id", CustomerId);
            ja.Add("identity");
            ja.Add("contact");
            ja.Add("socialAccount");
            ja.Add("controlInfo");

            jo.Add("data", ja);

            int maximunRetryCount = 1;
            JObject jObject = null;

            for (int i = 0; i < maximunRetryCount; i++)
            {
                string resultJson = "";
                try
                {
                    resultJson = await httpHelper_.HttpPostAsync(url, JsonConvert.SerializeObject(jo), "application/json", 600, null);
                    jObject = JsonConvert.DeserializeObject<JObject>(resultJson);
                    JToken value = "";
                    if (jObject.TryGetValue("errcode", out value))
                    {
                        log_.LogWarning(String.Format("Fail to get the Info of this Customer.ResultJson is {0}.CustomerId = {1}.Retry Count = {2}", resultJson, CustomerId, i));
                        jObject = new JObject();
                        jObject.Add("cust_id", CustomerId);
                    }
                    else
                    {
                        break;
                    }
                    if (jObject == null || jObject.GetValue("cust_id") == null)
                    {
                        log_.LogWarning(String.Format("The result message is null.CustomerId = {0}", CustomerId));
                    }
                    else
                    {
                        log_.LogDebug(String.Format("Response Message is:{0}", jObject.ToString()));
                    }

                }
                catch (Exception e)
                {
                    log_.LogError(e, "Get CustomerInfo failed.CustomerId=" + CustomerId + " resultJson = " + resultJson, null);
                }
            }

            log_.LogInformation(String.Format("End getCustomerRunner.CustomerId = {0}", CustomerId));
            return jObject;
        }

        //GetCusInfo
        [HttpGet("owner")]
        public async Task<IActionResult> getOwnerInfos()
        {
            log_.LogInformation("Enter getOwnerInfos");
            log_.LogInformation("using account is " + json_csv_account);
            ResponseObject ro = new ResponseObject();
            try
            {
                var cacheFile = ifOwnerInfoCacheExists();
                if (cacheFile != null)
                {
                    return cacheFile;
                }
                string token = await getToken();

                if (String.IsNullOrEmpty(token))
                {
                    ro.Result = "Error";
                    ro.ErrorMessage = "token is null or empty.";
                }
                else
                {
                    List<string> customerIds = new List<string>();
                    List<string> tempIds = new List<string>();
                    string nextCusId = "";
                    int totalCount = 0;
                    do
                    {
                        //get All cusomterId 
                        string url = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getCustList?next_custid=" + nextCusId + "&access_token=" + token;
                        string customersJson = await httpHelper_.HttpGetAsync(url, null);
                        var CustomersResultDefinition = new { total = "", count = "", data = new { cust_id = new List<string>() }, next_custid = "" };
                        var retJsonObject = JsonConvert.DeserializeAnonymousType(customersJson, CustomersResultDefinition);

                        if (retJsonObject == null || retJsonObject.data == null || retJsonObject.data.cust_id == null)
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "CustomersResultDefinition == null || CustomersResultDefinition.data || CustomersResultDefinition.data.cust_id == null";
                            ro.ResultObject = retJsonObject;
                            return Ok(ro);
                        }

                        nextCusId = retJsonObject.next_custid;
                        int tempCount = 0;
                        if (!int.TryParse(retJsonObject.count, out tempCount))
                        {
                            ro.Result = "Error";
                            ro.ErrorMessage = "Total Count is not a integer.";
                            return Ok(ro);
                        }

                        totalCount = totalCount + tempCount;

                        tempIds = retJsonObject.data.cust_id;
                        customerIds.AddRange(tempIds);
                    } while (!String.IsNullOrEmpty(nextCusId));

                    if (totalCount != customerIds.Count)
                    {
                        ro.Result = "Error";
                        ro.ErrorMessage = "Total Count not tally with the records count in the result object.";
                        return Ok(ro);
                    }

                    ConcurrentQueue<JObject> returnArray = new ConcurrentQueue<JObject>();
                    log_.LogDebug("Before get owner info reccurlly");
                    List<Task> tasks = new List<Task>();
                    int idx = 0;
                    int limitCount = 500;
                    //customerIds = customerIds.GetRange(0, 100);

                    log_.LogInformation(String.Format("Total customer count is {0}",customerIds.Count));

                    foreach (string cusId in customerIds)
                    {
                        idx++;
                        log_.LogDebug(String.Format("Start task {0}.CusId = {1}", idx, cusId));
                        tasks.Add(getOwnerRunner(cusId, token));

                        if (idx % limitCount == 0)
                        {
                            log_.LogInformation("到达阈值，现在的idx=" + idx);
                            //等待该批任务全部完成，然后进入下一批

                            foreach (Task<JObject> t in tasks)
                            {
                                var ret = await t;
                                returnArray.Enqueue(ret);
                            }

                            //清空该批已经完成的task
                            tasks.Clear();
                            //休息6秒
                            Thread.Sleep(6000);
                        }
                    }

                    foreach (Task<JObject> t in tasks)
                    {
                        var ret = await t;
                        returnArray.Enqueue(ret);
                    }

                    await Task.WhenAll(tasks).ContinueWith(p =>
                    {
                        log_.LogDebug("After get owner info reccurlly");
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);


                    string email = json_csv_account;
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

                    string csvContent = await httpHelper_.HttpPostAsync(jsoncsvurl, encodedContent, null, 600);

                    if (csvContent.IndexOf("A problem occured") != -1)
                    {
                        ro.ErrorMessage = csvContent;
                        ro.Result = "Error";
                    }
                    else
                    {
                        System.Text.Encoding encoding = new System.Text.UTF8Encoding(true);
                        byte[] byteArray = encoding.GetBytes(csvContent);
                        string fileName = String.Format("ownerInfoJson_{0}.csv", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now));
                        fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
                        var data = Encoding.UTF8.GetPreamble().Concat(byteArray).ToArray();

                        cacheTheOwnerInfoFile(data,fileName);

                        log_.LogInformation("End getOwnerInfos");
                        return File(data, "application/csv", fileName);
                    }
                }

            }
            catch (TaskCanceledException te)
            {
                ro.ErrorMessage = te.Message;
                ro.Result = "Error";
                log_.LogError(te, "TaskCanceledException Occure.", null);
            }
            catch (Exception e)
            {
                ro.ErrorMessage = e.Message;
                ro.Result = "Error";
                log_.LogError(e, "Exception Occure.", null);
            }

            log_.LogInformation("Leave getOwnerInfos");
            return Ok(ro);
        }


        private async Task<JObject> getOwnerRunner(string OwnerID, string token)
        {
            Random r = new Random();
            int randomSleepTime = r.Next(1, 5);
            log_.LogInformation(String.Format("Start getOwnerRunner.OwnerId = {0}", OwnerID));
            string urlbase = "https://api.qidian.qq.com/cgi-bin/cust/cust_info/getSingCustBusiInfo?cust_id={0}&access_token={1}";
            string url = String.Format(urlbase, OwnerID, token);

            int maximunRetryCount = 1;
            JObject jObject = null;

            for (int i = 0; i < maximunRetryCount; i++)
            {
                try
                {
                    var resultJson = await httpHelper_.HttpPostAsync(url, "", "application/json", 600, null);
                    jObject = JsonConvert.DeserializeObject<JObject>(resultJson);
                    JToken value = "";
                    if (jObject.TryGetValue("errcode", out value))
                    {
                        log_.LogWarning(String.Format("Fail to get the Info of this Owner.OwnerId = {0}.Retry Count = {1}", OwnerID, i));
                        jObject = new JObject();
                        jObject.Add("cust_id", OwnerID);
                    }
                    else
                    {
                        break;
                    }

                }
                catch (Exception e)
                {
                    log_.LogError(e,"Get OwnerInfo failed.OwnerId=" + OwnerID, null);
                }
            }

            log_.LogInformation(String.Format("End getOwnerRunner.OwnerId = {0}", OwnerID));
            return jObject;
        }
    }
}
