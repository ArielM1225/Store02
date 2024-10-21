using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store02;
using Store02.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest("Product is null.");
            }

            if (string.IsNullOrEmpty(productDTO.NameProduct) ||
                string.IsNullOrEmpty(productDTO.DescriptionProduct) ||
                productDTO.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                var product = new Product
                {
                    NameProduct = productDTO.NameProduct,
                    DescriptionProduct = productDTO.DescriptionProduct,
                    Price = productDTO.Price,
                    Stock = 0
                };

                // Llamar al repositorio para crear el product
                _productRepository.CreateProduct(product);

                // Devolver un resultado exitoso
                return Ok("Product created successfully");
            }
            catch (Exception ex)
            {
                // Manejar posibles errores
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdatePrice/{productID}")]
        public IActionResult UpdateProductPrice(int productID, [FromBody] ProductUpdatePriceDTO productUpdate)
        {
            if (productUpdate == null || productUpdate.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {

                // Llamar al repositorio para actualizar el precio
                bool isUpdated = _productRepository.UpdatePrice(productID, productUpdate.Price);
                if (isUpdated)
                {
                    return Ok("Product price updated successfully");
                }
                else
                {
                    return NotFound("Product not found");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateData/{productID}")]
        public IActionResult UpdateProductData(int productID, [FromBody] ProductUpdateDataDTO productUpdateData)
        {
            if (productUpdateData == null ||
                string.IsNullOrEmpty(productUpdateData.NameProduct) ||
                string.IsNullOrEmpty(productUpdateData.DescriptionProduct))
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                // Llamar al repositorio para actualizar la data
                bool isUpdated = _productRepository.UpdateData(productID, productUpdateData.NameProduct, productUpdateData.DescriptionProduct);
                if (isUpdated)
                {
                    return Ok("Product data updated successfully");
                }
                else
                {
                    return NotFound("Product not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
