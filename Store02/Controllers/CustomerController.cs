using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store02.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [Authorize]  // Protege el endpoint
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                _customerRepository.CreateCustomer(customer);
                return Ok("Customer created successfully");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);  // 409 Conflict
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  // 500 Internal Server Error
            }
        }

        //[HttpPut("{id}")]
        //public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        //{
        //    // Lógica para actualizar los datos del cliente
        //    return Ok("Customer updated successfully");
        //}

    }
}