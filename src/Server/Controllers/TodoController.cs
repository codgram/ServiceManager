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
using ServiceManager.Client.Services;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ServiceManagerContext _context;
        private readonly UserManager<ServiceManagerUser> _userManager;

        public TodoController(ServiceManagerContext context, UserManager<ServiceManagerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public CompanyFilter CompanyFilter { get; set; }
        // GET: api/Todo/{companyName}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodo([FromQuery] string companyId)
        {
            return await _context.TodoItem.Where(t => t.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodo(string id, string companySlug)
        {
            var Todo = await _context.TodoItem.FindAsync(id);

            if (Todo == null)
            {
                return NotFound();
            }

            return Todo;
        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(string id, TodoItem todoItem)
        {
            if (id != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
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

        // POST: api/Todo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [BindProperty]
        public TodoItem TodoItem { get; set; }
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodo(TodoItem todoItem, [FromQuery] string companyId)
        {
            string userId = _userManager.GetUserId(User);


            var company = await _context.Company.FirstOrDefaultAsync(c => c.CompanyId == companyId);

            todoItem.ServiceManagerUserId = userId;
            todoItem.CompanyId = company.CompanyId;

            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todoItem.TodoItemId }, todoItem);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodo(string id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoExists(string id)
        {
            return _context.TodoItem.Any(e => e.TodoItemId == id);
        }
    }
}
