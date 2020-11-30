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

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public ProductGroupController(ServiceManagerContext context)
        {
            _context = context;
        }


        // GET: api/productgroup?companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGroup>>> GetProductGroup([FromQuery] string companyId)
        {
            return await _context.ProductGroup.Include(d => d.Company).Where(c => c.CompanyId == companyId).ToListAsync();
        }

        // GET: api/productgroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGroup>> GetProductGroupById(string id)
        {
            
            var ProductGroup = await _context.ProductGroup.FirstOrDefaultAsync(p => p.ProductGroupId == id);

            if (ProductGroup == null)
            {
                return NotFound();
            }

            return ProductGroup;
        }

        // PUT: api/productgroup/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductGroup(string id, ProductGroup productGroup)
        {
            if (id != productGroup.ProductGroupId)
            {
                return BadRequest();
            }

            _context.Entry(productGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductGroupExists(id))
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

        // POST: api/productgroup?companyId={companyId}
        [HttpPost]
        public async Task<ActionResult<ProductGroup>> PostProductGroup(ProductGroup productGroup, [FromQuery] string companyId)
        {
            productGroup.CompanyId = companyId;

            _context.ProductGroup.Add(productGroup);
            await _context.SaveChangesAsync();


            return CreatedAtAction($"GetProductGroup", new { id = productGroup.ProductGroupId, companyId = productGroup.CompanyId }, productGroup);
        }

        // DELETE: api/productgroup/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductGroup>> DeleteProductGroup(string id)
        {
            var productGroup = await _context.ProductGroup.FindAsync(id);
            if (productGroup == null)
            {
                return NotFound();
            }

            _context.ProductGroup.Remove(productGroup);
            await _context.SaveChangesAsync();

            return productGroup;
        }

        private bool ProductGroupExists(string id)
        {
            return _context.ProductGroup.Any(e => e.ProductGroupId == id);
        }
    }
}
