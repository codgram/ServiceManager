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
    public class VariantImageController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public VariantImageController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/VariantImage?companyId{companyId}
        [HttpGet]
        [EnableCors("Development")]
        public async Task<ActionResult<IEnumerable<VariantImage>>> GetVariantImage([FromQuery] string companyId, [FromQuery] bool productIsAcive)
        {
            if(productIsAcive == true) {
                return await _context.VariantImage.Where(v => v.Variant.Product.CompanyId == companyId && v.Variant.Product.IsActive == true).ToListAsync();
            }
            else {
                return await _context.VariantImage.Where(v => v.Variant.Product.CompanyId == companyId).ToListAsync();
            }
            
        }


        // GET: api/VariantImage/{variantId}?productId={productId}
        [HttpGet("{variantId}")]
        public async Task<ActionResult<IEnumerable<VariantImage>>> GetVariantImageByProduct([FromQuery] string productId, string variantId)
        {
            return await _context.VariantImage.Where(v => v.Variant.ProductId == productId && v.VariantId == variantId).ToListAsync();
        }

        // GET: api/VariantImage/5
        [HttpGet("{productId}/{id}")]
        public async Task<ActionResult<VariantImage>> GetVariantImageById(string id, string productId)
        {
            var VariantImage = await _context.VariantImage.FindAsync(id);

            if (VariantImage == null)
            {
                return NotFound();
            }

            return VariantImage;
        }

        // PUT: api/VariantImage/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariantImage(string id, VariantImage VariantImage)
        {
            if (id != VariantImage.VariantImageId)
            {
                return BadRequest();
            }

            _context.Entry(VariantImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantImageExists(id))
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

        // POST: api/VariantImage?variantId={variantId}
        [HttpPost]
        public async Task<ActionResult<VariantImage>> PostVariantImage(VariantImage variantImage, [FromQuery] string variantId)
        {
            variantImage.VariantId = variantId;
            _context.VariantImage.Add(variantImage);
            await _context.SaveChangesAsync();


            return CreatedAtAction($"GetVariantImage", new { id = variantImage.VariantImageId }, variantImage);
        }

        // DELETE: api/VariantImage/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VariantImage>> DeleteVariantImage(string id)
        {
            var VariantImage = await _context.VariantImage.FindAsync(id);
            if (VariantImage == null)
            {
                return NotFound();
            }

            _context.VariantImage.Remove(VariantImage);
            await _context.SaveChangesAsync();

            return VariantImage;
        }

        private bool VariantImageExists(string id)
        {
            return _context.VariantImage.Any(e => e.VariantImageId == id);
        }
    }
}
