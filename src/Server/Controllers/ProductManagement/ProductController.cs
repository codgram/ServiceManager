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
using Microsoft.AspNetCore.Cors;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public ProductController(ServiceManagerContext context)
        {
            _context = context;
        }


        // GET: api/product?companyId={companyId}
        [HttpGet]
        [EnableCors("Development")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct([FromQuery] string companyId)
        {
            return await _context.Product.Include(d => d.Company).Where(p => p.CompanyId == companyId).ToListAsync(); 
        }
        
        // GET: api/product/n/{productNo}?companyId={companyId}
        // [HttpGet("n/{productNo}")]
        // [EnableCors("Development")]
        // public async Task<ActionResult<Product>> GetProductByProductNo([FromQuery] string companyId, string productNo)
        // {
        //     var Product = await _context.Product.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductNo == productNo);

        //     if (Product == null)
        //     {
        //         return NotFound();
        //     }

        //     return Product;
        // }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var Product = await _context.Product.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Product;
        }

        // PUT: api/product/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/product/{companyId}
        public Variant Variant { get; set; }
        [HttpPost("{companyId}")]
        public async Task<ActionResult<Product>> PostProduct(Product product, string companyId)
        {
            product.CompanyId = companyId;
            product.IsActice = true;


            
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            Variant variant = new Variant() {
                ProductId = product.ProductId,
                VariantNo="V0001",
                Type = VariantType.Regular,
                Variation = "None"
            };
            _context.Variant.Add(variant);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction($"GetProduct", new { id = product.ProductId, companyId = product.CompanyId }, product);
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var variants = await _context.Variant.Where(v => v.ProductId == id).ToListAsync();
            var images = await _context.VariantImage.Where(v => v.Variant.ProductId == id).ToListAsync();
            var costs = await _context.ProductCost.Where(v => v.Variant.ProductId == id).ToListAsync();
            var prices = await _context.ProductPrice.Where(v => v.Variant.ProductId == id).ToListAsync();
            
            
            

            // Delete all the product prices
            foreach(var item in prices) {
                _context.ProductPrice.Remove(item);
                await _context.SaveChangesAsync();
            }

            // Delete all the product costs
            foreach(var item in costs) {
                _context.ProductCost.Remove(item);
                await _context.SaveChangesAsync();
            }

            // Delete all the product images
            foreach(var item in images) {
                _context.VariantImage.Remove(item);
                await _context.SaveChangesAsync();
            }

            // Delete all the variants
            foreach(var item in variants) {
                _context.Variant.Remove(item);
                await _context.SaveChangesAsync();
            }

            // Delete the product
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
