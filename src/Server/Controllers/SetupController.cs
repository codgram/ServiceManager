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

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]/documents")]
    //[Authorize]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public SetupController(ServiceManagerContext context)
        {
            _context = context;
        }


        // GET: api/setup/documents?companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentSetup>>> GetDocumentSetup([FromQuery] string companyId)
        {
            return await _context.DocumentSetup.Include(d => d.Company).Where(c => c.CompanyId == companyId).ToListAsync();
        }

        // GET: api/setup/documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentSetup>> GetDocumentSetupById(string id)
        {
            var DocumentSetup = await _context.DocumentSetup.FindAsync(id);

            if (DocumentSetup == null)
            {
                return NotFound();
            }

            return DocumentSetup;
        }

        // PUT: api/setup/documents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentSetup(string id, DocumentSetup DocumentSetup)
        {
            if (id != DocumentSetup.DocumentSetupId)
            {
                return BadRequest();
            }

            _context.Entry(DocumentSetup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentSetupExists(id))
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

        // POST: api/setup/documents?companyId{companyId}
        [HttpPost]
        public async Task<ActionResult<DocumentSetup>> PostDocumentSetup(DocumentSetup documentSetup, [FromQuery] string companyId)
        {
            documentSetup.CompanyId = companyId;

            _context.DocumentSetup.Add(documentSetup);
            await _context.SaveChangesAsync();


            return CreatedAtAction($"GetDocumentSetup", new { id = documentSetup.DocumentSetupId, companyId = documentSetup.CompanyId }, documentSetup);
        }

        // DELETE: api/setup/documents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DocumentSetup>> DeleteDocumentSetup(string id)
        {
            var DocumentSetup = await _context.DocumentSetup.FindAsync(id);
            if (DocumentSetup == null)
            {
                return NotFound();
            }

            _context.DocumentSetup.Remove(DocumentSetup);
            await _context.SaveChangesAsync();

            return DocumentSetup;
        }

        private bool DocumentSetupExists(string id)
        {
            return _context.DocumentSetup.Any(e => e.DocumentSetupId == id);
        }
    }
}
