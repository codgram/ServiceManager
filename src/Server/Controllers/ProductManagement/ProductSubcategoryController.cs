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
    public class ProductSubcategoryController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public ProductSubcategoryController(ServiceManagerContext context)
        {
            _context = context;
        }


        // GET: api/ProductSubcategory/companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSubcategory>>> GetProductSubcategory([FromQuery] string companyId)
        {
            return await _context.ProductSubcategory.Include(p => p.ProductCategory).ThenInclude(p => p.ProductGroup).Where(c => c.ProductCategory.ProductGroup.CompanyId == companyId).ToListAsync();
        }

        // GET: api/ProductSubcategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSubcategory>> GetProductSubcategoryById(string id)
        {
            var ProductSubcategory = await _context.ProductSubcategory.FindAsync(id);

            if (ProductSubcategory == null)
            {
                return NotFound();
            }

            return ProductSubcategory;
        }

        // PUT: api/ProductSubcategory/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSubcategory(string id, ProductSubcategory productSubcategory)
        {
            if (id != productSubcategory.ProductSubcategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productSubcategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSubcategoryExists(id))
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

        // POST: api/ProductSubcategory
        [HttpPost]
        public async Task<ActionResult<ProductSubcategory>> PostProductSubcategory(ProductSubcategory productSubcategory)
        {

            _context.ProductSubcategory.Add(productSubcategory);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetProductSubcategory", new { id = productSubcategory.ProductSubcategoryId }, productSubcategory);
        }

        // DELETE: api/ProductSubcategory/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductSubcategory>> DeleteProductSubcategory(string id)
        {
            var productSubcategory = await _context.ProductSubcategory.FindAsync(id);
            if (productSubcategory == null)
            {
                return NotFound();
            }

            Product product = await _context.Product.FirstOrDefaultAsync(p => p.ProductSubcategoryId == productSubcategory.ProductSubcategoryId);

            if(product == null) {
                _context.ProductSubcategory.Remove(productSubcategory);
                await _context.SaveChangesAsync();
            }
            

            return productSubcategory;
        }

        private bool ProductSubcategoryExists(string id)
        {
            return _context.ProductSubcategory.Any(e => e.ProductSubcategoryId == id);
        }
    }
}
