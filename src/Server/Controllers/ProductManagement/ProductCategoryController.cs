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
    public class ProductCategoryController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public ProductCategoryController(ServiceManagerContext context)
        {
            _context = context;
        }


        // GET: api/ProductCategory?companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategory([FromQuery] string companyId)
        {
            return await _context.ProductCategory.Include(d => d.ProductGroup).Where(c => c.ProductGroup.CompanyId == companyId).ToListAsync();
        }

        // GET: api/ProductCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategoryById(string id)
        {
            var ProductCategory = await _context.ProductCategory.FindAsync(id);

            if (ProductCategory == null)
            {
                return NotFound();
            }

            return ProductCategory;
        }

        // PUT: api/ProductCategory/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(string id, ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategory
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            
            _context.ProductCategory.Add(productCategory);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // DELETE: api/ProductCategory/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCategory>> DeleteProductCategory(string id)
        {
            var productCategory = await _context.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategory.Remove(productCategory);
            await _context.SaveChangesAsync();

            return productCategory;
        }

        private bool ProductCategoryExists(string id)
        {
            return _context.ProductCategory.Any(e => e.ProductCategoryId == id);
        }
    }
}
