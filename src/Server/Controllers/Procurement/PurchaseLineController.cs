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

namespace ServiceManager.Server.Controllers.Procurement
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PurchaseLineController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public PurchaseLineController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseLine?companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseLine>>> GetPurchaseLine([FromQuery] string companyId)
        {
            return await _context.PurchaseLine.Where(p => p.PurchaseHeader.CompanyId == companyId).ToListAsync();
        }

        // GET: api/PurchaseLine/h/5
        [HttpGet("h/{purchaseHeaderId}")]
        public async Task<ActionResult<IEnumerable<PurchaseLine>>> GetPurchaseLineByOrder(string purchaseHeaderId)
        {
            return await _context.PurchaseLine.Include(p => p.PurchaseHeader).Include(p => p.Product).Include(p => p.Variant).Where(p => p.PurchaseHeaderId == purchaseHeaderId).ToListAsync();
        }


        // GET: api/PurchaseLine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseLine>> GetPurchaseLineById(string id)
        {
            var PurchaseLine = await _context.PurchaseLine.FindAsync(id);

            if (PurchaseLine == null)
            {
                return NotFound();
            }

            return PurchaseLine;
        }

        // PUT: api/PurchaseLine/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseLine(string id, PurchaseLine PurchaseLine)
        {
            if (id != PurchaseLine.PurchaseLineId)
            {
                return BadRequest();
            }

            _context.Entry(PurchaseLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseLineExists(id))
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

        // POST: api/PurchaseLine
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PurchaseLine>> PostPurchaseLine(PurchaseLine PurchaseLine)
        {
            _context.PurchaseLine.Add(PurchaseLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseLine", new { id = PurchaseLine.PurchaseLineId }, PurchaseLine);
        }

        // DELETE: api/PurchaseLine/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseLine>> DeletePurchaseLine(string id)
        {
            var PurchaseLine = await _context.PurchaseLine.FindAsync(id);
            if (PurchaseLine == null)
            {
                return NotFound();
            }

            _context.PurchaseLine.Remove(PurchaseLine);
            await _context.SaveChangesAsync();

            return PurchaseLine;
        }

        private bool PurchaseLineExists(string id)
        {
            return _context.PurchaseLine.Any(e => e.PurchaseLineId == id);
        }

        private async Task<int> SetLineNo(string companyId) {
            var lastPurchaseLine = await _context.PurchaseLine.Include(p => p.PurchaseHeader).LastOrDefaultAsync(p => p.PurchaseHeader.CompanyId == companyId);
            return lastPurchaseLine.LineNo + 1;
        }

        
    }
}
