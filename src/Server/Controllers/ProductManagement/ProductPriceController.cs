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
    public class ProductPriceController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public ProductPriceController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/ProductPrice?companyId{companyId}
        [HttpGet]
        [EnableCors("Development")]
        public async Task<ActionResult<IEnumerable<ProductPrice>>> GetProductPrice([FromQuery] string companyId)
        {
            return await _context.ProductPrice.Where(v => v.Variant.Product.CompanyId == companyId).ToListAsync();
        }


        // GET: api/ProductPrice/{variantId}
        [HttpGet("{variantId}")]
        public async Task<ActionResult<IEnumerable<ProductPrice>>> GetProductPriceByProduct(string variantId)
        {
            return await _context.ProductPrice.Where(v => v.VariantId == variantId).ToListAsync();
        }

        // GET: api/ProductPrice/5
        [HttpGet("{productId}/{id}")]
        public async Task<ActionResult<ProductPrice>> GetProductPriceById(string id, string productId)
        {
            var ProductPrice = await _context.ProductPrice.FindAsync(id);

            if (ProductPrice == null)
            {
                return NotFound();
            }

            return ProductPrice;
        }

        // PUT: api/ProductPrice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductPrice(string id, ProductPrice ProductPrice)
        {
            if (id != ProductPrice.ProductPriceId)
            {
                return BadRequest();
            }

            _context.Entry(ProductPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductPriceExists(id))
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

        // POST: api/ProductPrice/{productId}
        [HttpPost("{productId}")]
        public async Task<ActionResult<ProductPrice>> PostProductPrice(ProductPrice productPrice, string productId, [FromQuery] string variantId)
        {
            productPrice.VariantId = variantId;
            _context.ProductPrice.Add(productPrice);
            await _context.SaveChangesAsync();


            return CreatedAtAction($"GetProductPrice", new { id = productPrice.ProductPriceId }, productPrice);
        }

        // DELETE: api/ProductPrice/5
        [HttpDelete("/{id}")]
        public async Task<ActionResult<ProductPrice>> DeleteProductPrice(string id)
        {
            var ProductPrice = await _context.ProductPrice.FindAsync(id);
            if (ProductPrice == null)
            {
                return NotFound();
            }

            _context.ProductPrice.Remove(ProductPrice);
            await _context.SaveChangesAsync();

            return ProductPrice;
        }

        private bool ProductPriceExists(string id)
        {
            return _context.ProductPrice.Any(e => e.ProductPriceId == id);
        }
    }
}
