using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store02.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Authorize]  // Protege el endpoint
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

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

        [HttpPut("UpdateContact/{customerID}")]
        public IActionResult UpdateCustomerContact(int customerID, [FromBody] CustomerUpdateContactDTO customerUpdateContact)
        {
            if (customerUpdateContact == null ||
                string.IsNullOrEmpty(customerUpdateContact.Email) ||
                string.IsNullOrEmpty(customerUpdateContact.PhoneNumber))
            {
                return BadRequest("Invalid customer data.");
            }

            try
            {
                // Llamar al repositorio para actualizar el contacto del customer
                bool isUpdated = _customerRepository.UpdateContact(customerID, customerUpdateContact.Email, customerUpdateContact.PhoneNumber);
                if (isUpdated)
                {
                    return Ok("Customer data updated successfully");
                }
                else
                {
                    return NotFound("Customer not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}