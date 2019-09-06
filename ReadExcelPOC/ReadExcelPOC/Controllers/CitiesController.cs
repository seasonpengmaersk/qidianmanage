using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReadExcelPOC.Models;

namespace ReadExcelPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(DBContext context, ILogger<CitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCity()
        {
            return await _context.City.ToListAsync();
        }


        // GET: api/Cities
        [HttpGet("alias/{alias}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityByAlias(string alias)
        {
            _logger.LogInformation("Enter GetCityByAlias. Alias=" + (alias==null?"":alias));
            if (alias != null)
            {
                alias = alias.Trim();
            }

            if (String.IsNullOrEmpty(alias))
            {
                ResponseObject result = new ResponseObject();
                result.Result = "Error";
                result.ErrorMessage = "Alias is null or empty.";
                return Ok(result);
            }

            //int count  = await _context.City.Where(c => c.Alias.Contains(alias)).Distinct().GroupBy(c=>c.Alias).Select(m=>m.Max(c=>c.Alias)).CountAsync();



            var rawlist = await _context.City.Where(c => c.Alias.Contains(alias)).Distinct().ToListAsync();
            var cityList = rawlist.GroupBy(c => c.GEOID).SelectMany(t => t.Select((b, i) => new { b, i }).Where(m => m.i == 0)).Select(m => m.b);

            var temp = rawlist.GroupBy(c => c.GEOID).Select(t => t.Select((b, i) => new { b, i })).ToList();
            var temp2 = rawlist.GroupBy(c => c.GEOID).SelectMany(t => t.Select((b, i) => new { b, i })).ToList();

            ////string sql = String.Format("SELECT GEOID,StandardName,MAX(Alias) AS Alias from[dbo].[City] WHERE ALIAS like '%{0}%' GROUP BY GEOID, StandardName", alias);

            //string sql = "SELECT ID,countryName,GEOID,RKSTCode,CityName,StandardName,MAX(Alias) AS Alias,Status from[dbo].[City] WHERE ALIAS like @alias GROUP BY ID,countryName,GEOID,RKSTCode,CityName,StandardName,Status";


            //var sqlAlias = new SqlParameter("alias", "%" +alias + "%");

            //var cityList =await _context.City.FromSql(sql,sqlAlias).ToListAsync(); 

            int count = cityList.Count();

            if (count > 10)
            {
                ResponseObject result = new ResponseObject();
                result.Result = "Error";
                result.ErrorMessage = "Too many records.";
                _logger.LogWarning("Too many records.");
                return Ok(result);
            }

            if (cityList.Count() == 0)
            {
                ResponseObject result = new ResponseObject();
                result.Result = "Error";
                result.ErrorMessage = "Alias not found.";
                _logger.LogWarning("Alias not found.");
                return Ok(result);
            }

            if (cityList == null)
            {
                _logger.LogWarning("cityList is null.Alias not found.");
                return NotFound();
            }
            _logger.LogInformation("GetCityByAlias result=" + JsonConvert.SerializeObject(cityList));
            _logger.LogInformation("End GetCityByAlias");
            return Ok(cityList);
        }

        // GET: api/Cities
        [HttpGet("alias/param")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityByAliasParam([FromQuery]string alias)
        {
            return await GetCityByAlias(alias);
        }


        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            var city = await _context.City.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.City.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(Guid id)
        {
            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.City.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(Guid id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
