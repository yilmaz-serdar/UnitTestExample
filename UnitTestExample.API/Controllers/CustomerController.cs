using Microsoft.AspNetCore.Mvc;
using UnitTestExample.Data.Core;
using UnitTestExample.Data.Entities;

namespace UnitTestExample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();

            return Ok(customers);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            _customerService.Add(customer);

            return CreatedAtAction("GetById", new { id = customer.Id }, customer);
        }
    }
}
