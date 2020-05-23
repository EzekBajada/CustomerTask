using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CustomerTask.Models;

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

        //[HttpGet]
        //public async Task<IActionResult> GetCustomer(int id)
        //{

        //}
    }
}
