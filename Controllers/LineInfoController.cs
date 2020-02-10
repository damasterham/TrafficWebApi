using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrafikWebAPI.Models;

namespace TrafikWebAPI.Controllers
{
    // Would make sense to make various types of tests for each controller or entity
    // Unit tests could be made in project, and be woven into a CI/CD pipeline
    // Testing through HTTP would also be smart, possibly utilizing tools as Postman or Curl
    // Since there could arrise different issues when passing the data across a connection 
    // instead of just internally
    // Here we would also want to do using 
    // logging (for example when errors occur) 
    // metrics (for example a methods call amount, to see which are popular)
    // and exception handling for things most outward facing errors

    [Route("[controller]")]
    [ApiController]
    public class LineInfoController : ControllerBase
    {
        private readonly TrafficInfoContext _context;

        // Since we registered our data access in startup
        // we can just get it through dependency injection from the ServiceProvider
        // 
        public LineInfoController(TrafficInfoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get LineInfoEntries from a specific line
        /// </summary>
        /// <param name="line">The desired line</param>
        /// <param name="limit">The limit of entries returned</param>
        /// <returns></returns>
        [HttpGet("line/{line}/{limit:int}")]
        public async Task<ActionResult<IEnumerable<LineInfo>>> GetSpecificLineInfoEntries(string line, int limit)
        {
            // Using LINQ to essentially generate abstract queries, since it's a mainstay of EF Cores
            var specificLineInfoEntries = _context.LineInfoEntries
                .Where(x => x.Line == line)
                .OrderByDescending(x => x.Time)
                .Take(limit);

            // Just filtering the collection and returning it, but here we could also handle validation and
            // what to return on failure, such as appropriate HTTP codes

            return await specificLineInfoEntries.ToListAsync();
        }

        /// <summary>
        /// Gets LineInfoEntries between to specific points in time, including edge values
        /// </summary>
        /// <param name="fromUtc">Time from</param>
        /// <param name="toUtc">Time to</param>
        /// <returns></returns>
        [HttpGet("between/{fromUtc:datetime}/{toUtc:datetime}")]
        public async Task<ActionResult<IEnumerable<LineInfo>>> GetLineInfoEntriesWithinTimeSpan(DateTime fromUtc, DateTime toUtc)
        {
            var between = _context.LineInfoEntries
                .Where(x => x.Time >= fromUtc && x.Time <= toUtc)
                .OrderByDescending(x => x.Time);

            // Just filtering the collection and returning it, but here we could also handle validation and
            // what to return on failure, such as appropriate HTTP codes

            return await between.ToListAsync();
        }


        // Just the default scaffold implementations for the LineInfo entity
        #region Scaffolded Implementations

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineInfo>>> GetLineInfoEntries()
        {
            return await _context.LineInfoEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LineInfo>> GetLineInfo(int id)
        {
            var lineInfo = await _context.LineInfoEntries.FindAsync(id);

            if (lineInfo == null)
            {
                return NotFound();
            }

            return lineInfo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineInfo(int id, LineInfo lineInfo)
        {
            if (id != lineInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineInfoExists(id))
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

        // Just using the default implementation of posting a single entry
        [HttpPost]
        public async Task<ActionResult<LineInfo>> PostLineInfo(LineInfo lineInfo)
        {
            _context.LineInfoEntries.Add(lineInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLineInfo", new { id = lineInfo.Id }, lineInfo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<LineInfo>> DeleteLineInfo(int id)
        {
            var lineInfo = await _context.LineInfoEntries.FindAsync(id);
            if (lineInfo == null)
            {
                return NotFound();
            }

            _context.LineInfoEntries.Remove(lineInfo);
            await _context.SaveChangesAsync();

            return lineInfo;
        }

        private bool LineInfoExists(int id)
        {
            return _context.LineInfoEntries.Any(e => e.Id == id);
        }

        #endregion
    }
}