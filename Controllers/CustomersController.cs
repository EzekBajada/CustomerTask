using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CustomerTask.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection.Metadata.Ecma335;

namespace CustomerTask.Controllers
{
    // Helper class to take care of db context injection.
    public class InjectedController : ControllerBase
    {
        protected readonly CustomerContext db;

        public InjectedController(CustomerContext context)
        {
            db = context;
        }
    }

    [Microsoft.AspNetCore.Components.Route("/api/[controller]")]
    public class CustomersController : InjectedController
    {
        public CustomersController(CustomerContext context) : base(context) { }

        [HttpGet]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await db.Customers.FindAsync(id);

            if(customer == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} does not exist");
                return BadRequest(ModelState);
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCheck = db.Customers.FindAsync(customer.ID);
            
            if(customerCheck.Result.ID != 0)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} already exists");
                return BadRequest(ModelState);
            }

            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer([FromBody] Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCheck = db.Customers.FindAsync(customer.ID);
            if(customerCheck == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} does not exist");
                return BadRequest(ModelState);
            }

            customerCheck.Result.FullName = customer.FullName;
            customerCheck.Result.Details = customer.Details;
            customerCheck.Result.Position = customer.Position;

            db.Customers.Update(customerCheck.Result);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await db.Customers.FindAsync(id);

            if (customer == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} does not exist");
                return BadRequest(ModelState);
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }
    }
}
