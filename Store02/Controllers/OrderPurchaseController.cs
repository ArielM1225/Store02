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
    public class OrderPurchaseController : ControllerBase
    {

        private readonly OrderPurchaseRepository _orderPurchaseRepository;
        private readonly OrderPurchaseDetailRepository _orderPurchaseDetailRepository;

        public OrderPurchaseController(OrderPurchaseRepository orderPurchaseRepository, OrderPurchaseDetailRepository orderPurchaseDetailRepository)
        {
            _orderPurchaseRepository = orderPurchaseRepository;
            _orderPurchaseDetailRepository = orderPurchaseDetailRepository;
        }

        [HttpPost("create-order-with-details")] // Ruta específica
        public IActionResult CreateOrderPurchaseWithDetails([FromBody] OrderPurchaseWithDetailsDTO orderWithDetails)
        {
            // Validar si tiene al menos un detalle de la orden
            if (orderWithDetails.OrderPurchaseDetail == null || orderWithDetails.OrderPurchaseDetail.Count == 0)
            {
                return BadRequest("It requires at least one detail for the order.");
            }

            // Crear el order purchase (TotalAmount se inicializa en 0)
            var orderP = new OrderPurchase
            {
                SupplierID = orderWithDetails.SupplierID,
                StatusOrder = orderWithDetails.StatusOrder
            };

            int newOrderId = _orderPurchaseRepository.CreatOrderIn(orderP);

            // Crear los detalles de la orden y calcular el total
            decimal totalAmount = 0;
            foreach (var detail in orderWithDetails.OrderPurchaseDetail)
            {
                var orderPurchaseDetail = new OrderPurchaseDetail
                {
                    OrderPID = newOrderId,
                    ProductID = detail.ProductID,
                    Quantity = detail.Quantity,
                    Price = detail.Price
                };

                _orderPurchaseDetailRepository.CreateOrderPurchaseDetail(orderPurchaseDetail);

                // Sumar al total el importe del detalle (Quantity * Price)
                totalAmount += detail.Quantity * detail.Price;
            }

            // Actualizar el TotalAmount en el order
            _orderPurchaseRepository.UpdateTotalAmount(newOrderId, totalAmount);

            return Ok("OrderPurchase and OrderPurchaseDetail created successfully");
        }

        [HttpPost("add-order-detail")] // Otra ruta específica
        //public IActionResult AddOrderPurchaseDetail([FromBody] OrderPurchaseDetailDTO orderPurchaseDetailDTO)
        //{
        //    if (orderPurchaseDetailDTO == null)
        //    {
        //        return BadRequest("Order purchase detail is null.");
        //    }

        //    try
        //    {
        //        // Crear el modelo desde el DTO
        //        var orderPurchaseDetail = new OrderPurchaseDetail
        //        {
        //            OrderPID = orderPurchaseDetailDTO.OrderPID,
        //            ProductID = orderPurchaseDetailDTO.ProductID,
        //            Quantity = orderPurchaseDetailDTO.Quantity,
        //            Price = orderPurchaseDetailDTO.Price
        //        };

        //        // Verificar si se pudo agregar
        //        bool isAdded = _orderPurchaseDetailRepository.AddOrderPurchaseDetail(orderPurchaseDetail);
        //        if (!isAdded)
        //        {
        //            return BadRequest("Cannot add detail. The order is not in 'Pending' status.");
        //        }

        //        return Ok("Order purchase detail added successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        public IActionResult AddOrderPurchaseDetail([FromBody] OrderPurchaseDetailDTO orderPurchaseDetailDTO)
        {
            if (orderPurchaseDetailDTO == null)
            {
                return BadRequest("Order purchase detail is null.");
            }

            try
            {
                // Crear el modelo desde el DTO
                var orderPurchaseDetail = new OrderPurchaseDetail
                {
                    OrderPID = orderPurchaseDetailDTO.OrderPID,
                    ProductID = orderPurchaseDetailDTO.ProductID,
                    Quantity = orderPurchaseDetailDTO.Quantity,
                    Price = orderPurchaseDetailDTO.Price
                };

                // Verificar si se pudo agregar
                bool isAdded = _orderPurchaseDetailRepository.AddOrderPurchaseDetail(orderPurchaseDetail);
                if (!isAdded)
                {
                    return BadRequest("Cannot add detail. The order is not in 'Pending' status.");
                }

                return Ok("Order purchase detail added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("UpdateStatus/{orderPID}")]
        public IActionResult UpdateStatusOrder(int orderPID, [FromBody] OrderPurchaseUpdateStatusDTO orderUpdateStatus)
        {
            if (orderUpdateStatus == null ||
                string.IsNullOrEmpty(orderUpdateStatus.StatusOrder))
            {
                return BadRequest("Invalid status data.");
            }

            try
            {
                // Llamar al repositorio para actualizar el statusOrder
                bool isUpdated = _orderPurchaseRepository.UpdateStatus(orderPID, orderUpdateStatus.StatusOrder);
                if (isUpdated)
                {
                    return Ok("StatusOrder updated successfully");
                }
                else
                {
                    return NotFound("OrderPurchase not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
