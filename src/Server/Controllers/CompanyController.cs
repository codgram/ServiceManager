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

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ServiceManagerContext _context;
        private readonly UserManager<ServiceManagerUser> _userManager;

        public CompanyController(ServiceManagerContext context, UserManager<ServiceManagerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {
            string userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            
            var member = await _context.Member.Where(m => m.ServiceManagerUserId == userId).ToListAsync();
            var memberCompanies = member.Select(m => m.CompanyId).ToArray();
            return await _context.Company.Where(c => memberCompanies.Contains(c.CompanyId)).ToListAsync();
            //return await _context.Company.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyBy(string id)
        {
            var Company = await _context.Company.FindAsync(id);

            if (Company == null)
            {
                return NotFound();
            }

            return Company;
        }

        // GET: api/Company/5
        [HttpGet("s")]
        public async Task<ActionResult<Company>> GetCompanyBySlug([FromQuery] string slug)
        {
            var Company = await _context.Company.FirstOrDefaultAsync(c => c.Slug == slug);

            if (Company == null)
            {
                return NotFound();
            }

            return Company;
        }

        // PUT: api/Company/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(string id, Company Company)
        {
            if (id != Company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(Company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Company
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [BindProperty]
        public Member Member { get; set; }
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            if(!CompanySlugExists(company.Slug)) {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
    
                company.ServiceManagerUserId = user.Id;
                
                _context.Company.Add(company);
                await _context.SaveChangesAsync();
    
                
    
                Member member = new Member() {
                    CompanyId = company.CompanyId,
                    ServiceManagerUserId = company.ServiceManagerUserId
                };
    
                _context.Member.Add(member);
                await _context.SaveChangesAsync();

            }
            
            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(string id)
        {
            var Company = await _context.Company.FindAsync(id);
            if (Company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(Company);
            await _context.SaveChangesAsync();

            return Company;
        }

        private bool CompanyExists(string id)
        {
            return _context.Company.Any(e => e.CompanyId == id);
        }

        private bool CompanySlugExists(string slug)
        {
            return _context.Company.Any(e => e.Slug == slug);
        }
    }
}
