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
using ServiceManager.Shared.Models.Setup;
using ServiceManager.Shared.Models.ProductManagement;
using ServiceManager.Client.Services;
using Microsoft.AspNetCore.Cors;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public VariantController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/Variant?companyId{companyId}
        [HttpGet]
        [EnableCors("Development")]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariant([FromQuery] string companyId, [FromQuery] bool productIsActive)
        {
            if(productIsActive == true) {
                return await _context.Variant.Include(v => v.Product).Where(v => v.Product.CompanyId == companyId && v.Product.IsActive == true).ToListAsync();
            }
            else {
                return await _context.Variant.Include(v => v.Product).Where(v => v.Product.CompanyId == companyId).ToListAsync();
            }
            
        }


        // GET: api/Variant/{productId}
        [HttpGet("{productId}")]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariantByProduct(string productId)
        {
            return await _context.Variant.Where(v => v.ProductId == productId).ToListAsync();
        }

        // GET: api/Variant/{variantNo}?productId={productId}
        // [HttpGet("{variantNo}")]
        // public async Task<ActionResult<IEnumerable<Variant>>> GetVariantByVariantNo([FromQuery] string variantNo, string productId)
        // {
        //     return await _context.Variant.Where(v => v.ProductId == productId && v.VariantNo == variantNo).ToListAsync();
        // }

        // GET: api/Variant/5
        [HttpGet("{productId}/{id}")]
        public async Task<ActionResult<Variant>> GetVariantById(string id, string productId)
        {
            var Variant = await _context.Variant.FindAsync(id);

            if (Variant == null)
            {
                return NotFound();
            }

            return Variant;
        }

        // PUT: api/Variant/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariant(string id, Variant Variant)
        {
            if (id != Variant.VariantId)
            {
                return BadRequest();
            }

            _context.Entry(Variant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantExists(id))
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

        // POST: api/Variant/{productId}
        [HttpPost("{productId}")]
        public async Task<ActionResult<Variant>> PostVariant(Variant variant, string productId)
        {
            variant.VariantNo = await SetVariantNoAsync(productId);
            _context.Variant.Add(variant);
            await _context.SaveChangesAsync();


            return CreatedAtAction($"GetVariant", new { id = variant.VariantId }, variant);
        }

        // DELETE: api/Variant/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Variant>> DeleteVariant(string id)
        {
            var Variant = await _context.Variant.FindAsync(id);
            if (Variant == null)
            {
                return NotFound();
            }

            var variantImage = await _context.VariantImage.FirstOrDefaultAsync(v => v.VariantId == Variant.VariantId);

            if(variantImage == null) {
                _context.Variant.Remove(Variant);
                await _context.SaveChangesAsync();
            }
            

            return Variant;
        }

        private bool VariantExists(string id)
        {
            return _context.Variant.Any(e => e.VariantId == id);
        }

        private async Task<string> SetVariantNoAsync(string productId) {
            Variant lastVariant = await _context.Variant.OrderBy(v => v.VariantNo).LastOrDefaultAsync(v => v.ProductId == productId);
            int newVariantNo = Convert.ToInt32(lastVariant.VariantNo.Replace("V", "")) + 1;
            string variantNo = "";
            if(newVariantNo < 10) {
                variantNo = "V000" + newVariantNo;
            }
            else if(newVariantNo < 100) {
                variantNo = "V00" + newVariantNo;
            }
            else if(newVariantNo < 1000) {
                variantNo = "V0" + newVariantNo;
            }
            else {
                variantNo = "V" + newVariantNo;
            }
            return variantNo;
        }
    }
}
