using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Server.Data;
using ServiceManager.Shared.Models;
using ServiceManager.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public UserController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Areas.Identity.Data.ServiceManagerUser>>> GetUser()
        {
            return await _context.ServiceManagerUser.ToListAsync();
        }


        // GET: api/user/id={id}
        [HttpGet("id={Id}")]
        public async Task<ActionResult<Areas.Identity.Data.ServiceManagerUser>> GetUserById(string Id)
        {
            var user = await _context.ServiceManagerUser.FirstOrDefaultAsync(c => c.Id == Id);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/user/username={username}
        [HttpGet("username={username}")]
        public async Task<ActionResult<Areas.Identity.Data.ServiceManagerUser>> GetUserByUsername(string username)
        {
            var user = await _context.ServiceManagerUser.FirstOrDefaultAsync(c => c.UserName == username);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/user/email={email}
        [HttpGet("email={email}")]
        public async Task<ActionResult<Areas.Identity.Data.ServiceManagerUser>> GetUserByEmail(string email)
        {
            var user = await _context.ServiceManagerUser.FirstOrDefaultAsync(c => c.Email == email);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/user/companyId={companyId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Areas.Identity.Data.ServiceManagerUser>>> GetUserByCompany([FromQuery]string companyId)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.CompanyId == companyId);
            var members = await _context.Member.Where(m => m.CompanyId == company.CompanyId).ToListAsync();
            var membersArray = members.Select(m => m.ServiceManagerUserId).ToArray();
            
            return await _context.ServiceManagerUser.Where(s => membersArray.Contains(s.Id)).ToListAsync();
        }
        
        

        private bool UserExists(string id)
        {
            return _context.ServiceManagerUser.Any(e => e.Id == id);
        }
    }
}
