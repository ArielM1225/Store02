using Microsoft.AspNetCore.Mvc;
using Store02.Services;
using Store02.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CustomerLoginDTO customerLogin)
        {
            // Aquí iría la lógica para validar las credenciales del cliente (simulada en este caso)
            if (IsValidCustomer(customerLogin.Email, customerLogin.Password))  // Simula la validación
            {
                var token = _jwtService.GenerateJwtToken(customerLogin.Email);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private bool IsValidCustomer(string email, string password)
        {
            // Aquí deberías consultar tu base de datos para validar el email y el password del cliente
            // Esto es solo una simulación:
            return email == "ariemeneses243@gmail.com" && password == "prueba1225";
        }

    }
}