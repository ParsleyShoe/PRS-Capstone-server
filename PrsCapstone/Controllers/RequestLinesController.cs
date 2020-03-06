using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsCapstone.Models;

namespace PrsCapstone.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestLinesController : ControllerBase {
        private readonly PrsCapstoneContext _context;

        public RequestLinesController(PrsCapstoneContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines() {
            return await _context.RequestLines.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id) {
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine == null) {
                return NotFound();
            }

            return requestLine;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine) {
            if (id != requestLine.Id) {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RequestLineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine) {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestLine>> DeleteRequestLine(int id) {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null) {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            return requestLine;
        }

        private bool RequestLineExists(int id) {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
