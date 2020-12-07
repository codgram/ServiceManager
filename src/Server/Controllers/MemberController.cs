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
    [Authorize]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ServiceManagerContext _context;
        private readonly UserManager<ServiceManagerUser> _userManager;

        public MemberController(ServiceManagerContext context, UserManager<ServiceManagerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/member?companyId{companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMember([FromQuery] string companyId)
        {
            
            return await _context.Member.Include(m => m.Company).Where(m => m.CompanyId == companyId).ToListAsync();
        }

        // GET: api/member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMemberById(string id)
        {
            var Member = await _context.Member.FindAsync(id);

            if (Member == null)
            {
                return NotFound();
            }

            return Member;
        }

        // PUT: api/member/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(string id, Member member)
        {
            if (id != Member.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(Member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/member
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [BindProperty]
        public Member Member { get; set; }

        // POST: api/member?username={username}
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member, [FromQuery] string username)
        {

            var user = await _context.ServiceManagerUser.FirstOrDefaultAsync(s => s.UserName == username);

            if(user != null) {
                member.ServiceManagerUserId = user.Id;
                _context.Member.Add(member);
                await _context.SaveChangesAsync();
            }
            var company = await _context.Company.FirstOrDefaultAsync(c => c.CompanyId == member.CompanyId);
            

            return CreatedAtAction("GetMember", new { id = member.MemberId, companySlug = company.Slug }, member);
        }

        // DELETE: api/member/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> DeleteMember(string id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Member.Remove(member);
            await _context.SaveChangesAsync();

            return Member;
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }

        private Company GetCompany(string companySlug) {
            return _context.Company.FirstOrDefault(c => c.Slug == companySlug);
        }
    }
}
