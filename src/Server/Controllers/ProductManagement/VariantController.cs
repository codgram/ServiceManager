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
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariant([FromQuery] string companyId)
        {
            return await _context.Variant.Include(v => v.Product).Where(v => v.Product.CompanyId == companyId).ToListAsync();
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

            _context.Variant.Remove(Variant);
            await _context.SaveChangesAsync();

            return Variant;
        }

        private bool VariantExists(string id)
        {
            return _context.Variant.Any(e => e.VariantId == id);
        }
    }
}
