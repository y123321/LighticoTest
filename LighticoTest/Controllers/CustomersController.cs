using LighticoTest.Interfaces.Services;
using LighticoTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LighticoTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        ICustomerService _customersService;
        public CustomersController(ICustomerService customersService)
        {
            _customersService = customersService;
        }

        [HttpPost(Name = "AddCustomer")]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity();
            if (customer.Id != default(Guid))
                return BadRequest();
            await Task.Run(() =>
            {
                _customersService.AddCustomer(customer);

            });
            return Ok(customer);


        }

        // PUT: api/Users/5
        [HttpPut("{id}", Name = "UpdateCustomer")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Customer customer)
        {
            if (customer.Id != id)
                return BadRequest();
            if (!ModelState.IsValid || id == default(Guid))
                return UnprocessableEntity();
            await Task.Run(() =>
            {
                _customersService.UpdateCustomer(customer);
            });
            return Ok(customer);

        }
    }
}
