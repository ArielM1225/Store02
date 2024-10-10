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
            // Obtenemos el email del JWT token autenticado
            var customerEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (customerEmail == null)
            {
                return Unauthorized();
            }

            // Solo se permite crear si el cliente no ha sido registrado antes
            customer.Email = customerEmail;
            _customerRepository.CreateCustomer(customer);

            return Ok("Customer created successfully");
        }

        //[HttpPut("{id}")]
        //public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        //{
        //    // Lógica para actualizar los datos del cliente
        //    return Ok("Customer updated successfully");
        //}

    }
}