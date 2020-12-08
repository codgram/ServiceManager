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
    public class PurchaseHeaderController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public PurchaseHeaderController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseHeader?companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseHeader>>> GetPurchaseHeader([FromQuery] string companyId)
        {
            return await _context.PurchaseHeader.Include(p => p.Company).Where(p => p.CompanyId == companyId).ToListAsync();
        }

        // GET: api/PurchaseHeader/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseHeader>> GetPurchaseHeaderById(string id)
        {
            var PurchaseHeader = await _context.PurchaseHeader.FindAsync(id);

            if (PurchaseHeader == null)
            {
                return NotFound();
            }

            return PurchaseHeader;
        }

        // PUT: api/PurchaseHeader/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseHeader(string id, PurchaseHeader PurchaseHeader)
        {
            if (id != PurchaseHeader.PurchaseHeaderId)
            {
                return BadRequest();
            }

            _context.Entry(PurchaseHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseHeaderExists(id))
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

        // POST: api/PurchaseHeader
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PurchaseHeader>> PostPurchaseHeader(PurchaseHeader purchaseHeader, [FromQuery] string companyId)
        {
            purchaseHeader.PurchaseOrderNo = await SetPONo(companyId);
            purchaseHeader.CompanyId = companyId;
            _context.PurchaseHeader.Add(purchaseHeader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseHeader", new { id = purchaseHeader.PurchaseHeaderId }, purchaseHeader);
        }

        // DELETE: api/PurchaseHeader/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseHeader>> DeletePurchaseHeader(string id)
        {
            var purchaseHeader = await _context.PurchaseHeader.FindAsync(id);
            if (purchaseHeader == null)
            {
                return NotFound();
            }

            PurchaseLine line = await _context.PurchaseLine.FirstOrDefaultAsync(p => p.PurchaseHeaderId == purchaseHeader.PurchaseHeaderId);

            if(line == null) { 
                _context.PurchaseHeader.Remove(purchaseHeader);
                await _context.SaveChangesAsync();
            }
            

            return purchaseHeader;
        }

        private bool PurchaseHeaderExists(string id)
        {
            return _context.PurchaseHeader.Any(e => e.PurchaseHeaderId == id);
        }

        private async Task<string> SetPONo(string companyId)
        {

            PurchaseHeader lastPurchaseOrder = await _context.PurchaseHeader.LastOrDefaultAsync(p => p.CompanyId == companyId);
            DocumentSetup purchaseHeaderSetup = await _context.DocumentSetup.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.DocumentType == DocumentType.Purchase && p.Part == DocumentSetupPart.Prefix);
            string prefix = purchaseHeaderSetup.Content;
            string year = DateTime.Now.Year.ToString().Split("", 2)[1]; //gets the last 2 characters from the year

            if (lastPurchaseOrder == null)
            {
                return prefix + "-" + year + "-000001";
            }
            else
            {

                string[] poArray = lastPurchaseOrder.PurchaseOrderNo.Split("-");
                int no = Convert.ToInt32(poArray[poArray.Length - 1]) + 1; //the new suffix no
                string newnum = "";


                if (no < 10)
                {
                    newnum = "-00000" + no;
                }
                else if (no < 100)
                {
                    newnum = "-0000" + no;
                }
                else if (no < 1000)
                {
                    newnum = "-000" + no;
                }
                else if (no < 10000)
                {
                    newnum = "-00" + no;
                }
                else if (no < 100000)
                {
                    newnum = "-0" + no;
                }
                else if (no < 1000000)
                {
                    newnum = "-" + no;
                }
                else
                {
                    newnum = "contact Codgram";
                }


                return prefix + "-" + year + newnum;
            }


            
            

            
        }

        
    }
}
