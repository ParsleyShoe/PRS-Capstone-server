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
    public class VendorsController : ControllerBase {
        private readonly PrsCapstoneContext _context;

        public VendorsController(PrsCapstoneContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors() {
            return await _context.Vendors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id) {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null) {
                return NotFound();
            }

            return vendor;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor) {
            if (id != vendor.Id) {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!VendorExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor) {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(int id) {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return vendor;
        }

        private bool VendorExists(int id) {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
