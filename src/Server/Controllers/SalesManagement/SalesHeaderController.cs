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
using Microsoft.AspNetCore.Identity;
using ServiceManager.Server.Areas.Identity.Data;
using ServiceManager.Shared.Models.SalesManagement;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SalesHeaderController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public SalesHeaderController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/SalesHeader
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesHeader>>> GetSalesHeader([FromQuery] string companyId)
        {
            return await _context.SalesHeader.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        // GET: api/SalesHeader/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesHeader>> GetSalesHeaderById(string id)
        {
            var SalesHeader = await _context.SalesHeader.FindAsync(id);

            if (SalesHeader == null)
            {
                return NotFound();
            }

            return SalesHeader;
        }


        // PUT: api/SalesHeader/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesHeader(string id, SalesHeader SalesHeader)
        {
            if (id != SalesHeader.SalesHeaderId)
            {
                return BadRequest();
            }

            _context.Entry(SalesHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesHeaderExists(id))
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

        // POST: api/SalesHeader
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [BindProperty]
        public Member Member { get; set; }
        [HttpPost]
        public async Task<ActionResult<SalesHeader>> PostSalesHeader(SalesHeader SalesHeader)
        {
            
            _context.SalesHeader.Add(SalesHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesHeader", new { id = SalesHeader.SalesHeaderId }, SalesHeader);
        }

        // DELETE: api/SalesHeader/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalesHeader>> DeleteSalesHeader(string id)
        {
            var SalesHeader = await _context.SalesHeader.FindAsync(id);
            if (SalesHeader == null)
            {
                return NotFound();
            }

            _context.SalesHeader.Remove(SalesHeader);
            await _context.SaveChangesAsync();

            return SalesHeader;
        }

        private bool SalesHeaderExists(string id)
        {
            return _context.SalesHeader.Any(e => e.SalesHeaderId == id);
        }
    }
}
