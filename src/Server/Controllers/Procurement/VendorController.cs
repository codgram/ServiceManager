using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Server.Data;
using ServiceManager.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using ServiceManager.Shared.Models.Procurement;
using ServiceManager.Shared.Models.Setup;

namespace ServiceManager.Server.Controllers.Procurement
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public VendorController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/Vendor/{companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendor([FromQuery] string companyId)
        {
            return await _context.Vendor.Where(p => p.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Vendor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendorById(string id)
        {
            var Vendor = await _context.Vendor.FindAsync(id);

            if (Vendor == null)
            {
                return NotFound();
            }

            return Vendor;
        }

        // PUT: api/Vendor/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(string id, Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendor?companyId={companyId}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor, [FromQuery] string companyId)
        {
            vendor.CompanyId = companyId;
            _context.Vendor.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.VendorId }, vendor);
        }

        // DELETE: api/Vendor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(string id)
        {
            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            PurchaseHeader purchaseHeader = await  _context.PurchaseHeader.FirstOrDefaultAsync(p =>p.VendorId == vendor.VendorId);

            if(purchaseHeader == null) {
                _context.Vendor.Remove(vendor);
                await _context.SaveChangesAsync();
            }
            

            return vendor;
        }

        private bool VendorExists(string id)
        {
            return _context.Vendor.Any(e => e.VendorId == id);
        }
    }
}
