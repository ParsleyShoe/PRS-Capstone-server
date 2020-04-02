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

        [HttpGet("po/{vendorid}")]
        public async Task<ActionResult<IEnumerable<RequestLine>>> CreatePO(int vendorid) {
            var requestLines = await _context.RequestLines.Where(r => r.Request.Status == "APPROVED" && r.Product.VendorId == vendorid).ToListAsync();
            var selectedRequestLines = new List<RequestLine>();
            var doneIds = new List<int>();
            foreach (var reqline in requestLines) {
                int currentId = 0;
                if (!doneIds.Contains(reqline.ProductId)) {
                    doneIds.Add(reqline.ProductId);
                    currentId = reqline.ProductId;
                    int quantity = 0;
                    foreach (var reqline2 in requestLines) {
                        if (reqline2.ProductId == currentId) {
                            quantity += reqline2.Quantity;
                        }
                    }
                    reqline.Quantity = quantity;
                    selectedRequestLines.Add(reqline);
                }
            }
            return selectedRequestLines;

            //foreach (var rl in requestLines) {
            //    foreach (var rl2 in requestLines) {
            //        var itemToAdd = new RequestLine();
            //        if (rl.ProductId == rl2.ProductId) {
            //            if (rl.RequestId == rl2.RequestId) {

            //            }
            //            selectedRequestLines.Add(rl);
            //            continue;
            //        }
            //        if (rl.ProductId == rl2.ProductId && rl.RequestId != rl2.RequestId) {
            //            rl.Quantity += rl2.Quantity;
            //            selectedRequestLines.Add(rl);
            //        }
            //    }
            //}
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
