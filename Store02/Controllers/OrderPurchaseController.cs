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

        [HttpPost]
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

    }
}
