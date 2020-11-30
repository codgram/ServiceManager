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
using ServiceManager.Shared.Models.SalesManagement;

namespace ServiceManager.Server.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ServiceManagerContext _context;

        public CustomerController(ServiceManagerContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer([FromQuery] string companyId)
        {
            return await _context.Customer.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(string id)
        {
            var Customer = await _context.Customer.FindAsync(id);

            if (Customer == null)
            {
                return NotFound();
            }

            return Customer;
        }


        // PUT: api/Customer/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customer?companyId={companyId}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer, [FromQuery] string companyId)
        {
            customer.CompanyId = companyId;
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(string id)
        {
            var Customer = await _context.Customer.FindAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(Customer);
            await _context.SaveChangesAsync();

            return Customer;
        }

        private bool CustomerExists(string id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
