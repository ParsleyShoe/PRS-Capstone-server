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
    public class RequestsController : ControllerBase {
        private readonly PrsCapstoneContext _context;

        private async Task<bool> ChangeStatus(Request request, string change) {
            if (request == null) {
                return false;
            }
            request.Status = change;
            if (request.Total <= 50 && request.Status == "REVIEW") {
                request.Status = "APPROVED";
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public RequestsController(PrsCapstoneContext context) {
            _context = context;
        }

        [HttpGet("reviewbyuser/{userid}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsToReview(int userid) {
            return await _context.Requests.Where(r => r.Status == "REVIEW" && r.UserId != userid).ToListAsync();
        }

        [HttpPut("review/{id}")]
        public async Task<IActionResult> Review(int id) {
            if (!await ChangeStatus(_context.Requests.Find(id), "REVIEW")) {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id) {
            if (!await ChangeStatus(_context.Requests.Find(id), "APPROVED")) {
                return NotFound();
            }
            return NoContent();
        }

        // TODO still need Reject() method

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests() {
            return await _context.Requests.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            var request = await _context.Requests.FindAsync(id);

            if (request == null) {
                return NotFound();
            }

            return request;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if (id != request.Id) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!RequestExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request) {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id) {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return request;
        }

        private bool RequestExists(int id) {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
