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

        public void CalculateTotal(int requestid) {
            var request = _context.Requests.Find(requestid);
            if (request == null) {
                return;
            }
            request.Total = _context.RequestLines.Where(l => requestid == l.RequestId)
                                                 .Sum(l => l.Product.Price * l.Quantity);
            _context.SaveChanges();
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
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestline) {
            if (id != requestline.Id) {
                return BadRequest();
            }
            _context.Entry(requestline).State = EntityState.Modified;
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RequestLineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            CalculateTotal(requestline.RequestId);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestline) {
            _context.RequestLines.Add(requestline);
            await _context.SaveChangesAsync();
            CalculateTotal(requestline.RequestId);
            return CreatedAtAction("GetRequestLine", new { id = requestline.Id }, requestline);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestLine>> DeleteRequestLine(int id) {
            var requestline = await _context.RequestLines.FindAsync(id);
            if (requestline == null) {
                return NotFound();
            }
            _context.RequestLines.Remove(requestline);
            await _context.SaveChangesAsync();
            CalculateTotal(requestline.RequestId);
            return requestline;
        }

        private bool RequestLineExists(int id) {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
