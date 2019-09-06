using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReadExcelPOC.Models;
using ReadExcelPOC.Util.Common;

namespace ReadExcelPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<TerminalsController> _logger;

        public TerminalsController(DBContext context, ILogger<TerminalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Terminals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetTerminal()
        {
            return await _context.Terminal.ToListAsync();
        }


        // GET: api/Terminals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Terminal>> GetTerminal(string id)
        {
            Guid gid = new Guid(id);
            var terminal = await _context.Terminal.FindAsync(gid);

            if (terminal == null)
            {
                return NotFound();
            }

            return terminal;
        }



        [HttpGet("rkst/param")]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetTerminalByRKSTForm([FromQuery]string id)
        {

            //var terminalList = await _context.Terminal.Where(t => t.PortRKST == id).ToListAsync();
            //if (terminalList == null || terminalList.Count == 0)
            //{
            //    return NotFound();
            //}

            //return terminalList;
            return await GetTerminalByRKST(id);
        }



        [HttpGet("rkst/{id}")]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetTerminalByRKST(string id)
        {
            var terminalList = await _context.Terminal.Where(t => t.PortRKST == id).ToListAsync();
            if (terminalList == null || terminalList.Count == 0)
            {
                return NotFound();
            }

            return terminalList;
        }

        [HttpGet("geo/param")]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetTerminalByPortGeoIdPram([FromQuery]string id)
        {
            _logger.LogInformation("Enter GetTerminalByPortGeoIdPram.");
            var terminalList = await _context.Terminal.Where(t => t.PortGEOID == id).ToListAsync();

            if (terminalList == null || terminalList.Count == 0)
            {
                _logger.LogWarning("terminalList is not and terminalList.count ==0");
                return NotFound();
            }
            _logger.LogInformation("GetTerminalByPortGeoIdPram result is=" + JsonConvert.SerializeObject(terminalList));
            _logger.LogInformation("End GetTerminalByPortGeoIdPram.");
            return terminalList;
        }


        [HttpGet("geo/{id}")]
        public async Task<ActionResult<IEnumerable<Terminal>>> GetTerminalByPortGeoId(string id)
        {
            var terminalList = await _context.Terminal.Where(t => t.PortGEOID == id).ToListAsync();

            if (terminalList == null||terminalList.Count==0)
            {
                return NotFound();
            }

            return terminalList;
        }


        [HttpGet("track/param")]
        public async Task<ActionResult<IEnumerable<Terminal>>> trackPackage([FromQuery]string id)
        {
            _logger.LogInformation("Enter trackPackage.");
            string url = "https://api.maerskline.com/track/" + id;
            string ret =await HttpHelper.HttpGetAsync(url, null);
            _logger.LogInformation(String.Format("Return result is :{0}",ret));
            _logger.LogInformation("End trackPackage.");
            return Ok(ret);
        }


        [HttpGet("vessels/detail")]
        public async Task<ActionResult<IEnumerable<Terminal>>> vesselDetai([FromQuery]string id)
        {
            string url = "https://api.maerskline.com/maeu/vessels/details/" + id;
            string ret = await HttpHelper.HttpGetAsync(url, null);

            return Ok(ret);
        }



        // PUT: api/Terminals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerminal(string id, Terminal terminal)
        {
            if (id != terminal.TerminalGEOID)
            {
                return BadRequest();
            }

            _context.Entry(terminal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerminalExists(id))
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

        // POST: api/Terminals
        [HttpPost]
        public async Task<ActionResult<Terminal>> PostTerminal(Terminal terminal)
        {
            _context.Terminal.Add(terminal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTerminal", new { id = terminal.TerminalGEOID }, terminal);
        }

        // DELETE: api/Terminals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Terminal>> DeleteTerminal(string id)
        {
            var terminal = await _context.Terminal.FindAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }

            _context.Terminal.Remove(terminal);
            await _context.SaveChangesAsync();

            return terminal;
        }

        private bool TerminalExists(string id)
        {
            return _context.Terminal.Any(e => e.TerminalGEOID == id);
        }
    }
}
