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
using Microsoft.AspNetCore.Cors;

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

    public class CustomersController : InjectedController
    {
        

        public CustomersController(CustomerContext context) : base(context) { }

        [Microsoft.AspNetCore.Mvc.Route("/api/Allcustomers")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = db.Customers.ToList<Customers>();
            if(customers == null)
            {
                ModelState.AddModelError("Customers", $"No customers available");
                return BadRequest(ModelState);
            }
            return Ok(customers);
        }

        [Microsoft.AspNetCore.Mvc.Route("/api/GetCustomer/{customerId}")]
        [HttpGet]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            var customer = await db.Customers.FindAsync(customerId);

            if(customer == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customerId} does not exist");
                return BadRequest(ModelState);
            }

            return Ok(customer);
        }

        [Microsoft.AspNetCore.Mvc.Route("/api/Addcustomer")]
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCheck = db.Customers.FindAsync(customer.ID);
            
            if(customerCheck.Result != null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} already exists");
                return BadRequest(ModelState);
            }

            await db.Customers.AddAsync(customer);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }

        [Microsoft.AspNetCore.Mvc.Route("/api/EditCustomers")]
        [HttpPost]
        public async Task<IActionResult> EditCustomer([FromBody] Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerCheck = db.Customers.FindAsync(customer.ID);
            if(customerCheck.Result == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customer.ID} does not exist");
                return BadRequest(ModelState);
            }

            customerCheck.Result.FullName = customer.FullName;
            customerCheck.Result.Position = customer.Position;
            customerCheck.Result.Country = customer.Country;
            customerCheck.Result.Activity = customer.Activity;

            db.Customers.Update(customerCheck.Result);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }

        [Microsoft.AspNetCore.Mvc.Route("/api/DeleteCustomers/{customerId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            var customer = await db.Customers.FindAsync(customerId);

            if (customer == null)
            {
                ModelState.AddModelError("Customer ID", $"Customer {customerId} does not exist");
                return BadRequest(ModelState);
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return Ok(customer.ID);
        }

        [Microsoft.AspNetCore.Mvc.Route("/api/AddSomecustomers")]
        [HttpGet]
        public async Task<IActionResult> AddSomeCustomers()
        {
            Customers customer1 = new Customers()
            {
                ID = 1,
                FullName = "Hand Sanitizer",
                Position = "Tech Lead",
                Country = Customers.Countries.Malta,
                Activity = true
            };
            await db.Customers.AddAsync(customer1);
            Customers customer2 = new Customers()
            {
                ID = 2,
                FullName = "Hand-Wash sink",
                Position = "CEO",
                Country = Customers.Countries.Italy,
                Activity = true
            };
            await db.Customers.AddAsync(customer2);
            Customers customer3 = new Customers()
            {
                ID = 3,
                FullName = "Mask",
                Position = "Junior Developer",
                Country = Customers.Countries.England,
                Activity = true
            };
            await db.Customers.AddAsync(customer3);
            Customers customer4 = new Customers()
            {
                ID = 4,
                FullName = "Netflix",
                Position = "Human Resources",
                Country = Customers.Countries.England,
                Activity = false
            };
            await db.Customers.AddAsync(customer4);
            await db.SaveChangesAsync();
            return Ok("Changes have been added");
        }
    }
}
