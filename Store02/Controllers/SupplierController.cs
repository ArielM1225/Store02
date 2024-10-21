using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store02.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        
        private readonly SupplierRepository _supplierRepository;

        public SupplierController(SupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpPost]
        public IActionResult AddSupplier([FromBody] SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
            {
                return BadRequest("Supplier is null.");
            }

            if (string.IsNullOrEmpty(supplierDTO.SupplierName) ||
                string.IsNullOrEmpty(supplierDTO.ContactInfo))
            {
                return BadRequest("Invalid supplier data.");
            }

            try
            {
                var supplier = new Supplier
                {
                    SupplierName = supplierDTO.SupplierName,
                    ContactInfo = supplierDTO.ContactInfo
                };

                // Llamar al repositorio para crear el supplier
                _supplierRepository.CreateSupplier(supplier);

                // Devolver un resultado exitoso
                return Ok("Supplier created successfully.");
            }
            catch (Exception ex)
            {
                // Manejar posibles errores
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateData/{supplierID}")]
        public IActionResult UpdateSupplier(int supplierID, [FromBody] SupplierDTO supplierUpdateData)
        {
            if (supplierUpdateData == null ||
                string.IsNullOrEmpty(supplierUpdateData.SupplierName) ||
                string.IsNullOrEmpty(supplierUpdateData.ContactInfo))
            {
                return BadRequest("Invalid supplier data.");
            }

            try
            {
                // Llamar al repositorio para actualizar la data
                bool isUpdated = _supplierRepository.UpdateData(supplierID, supplierUpdateData.SupplierName, supplierUpdateData.ContactInfo);
                if (isUpdated)
                {
                    return Ok("Supplier data updated successfully");
                }
                else
                {
                    return NotFound("Supplier not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
