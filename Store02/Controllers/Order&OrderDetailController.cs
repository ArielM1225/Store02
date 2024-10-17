using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Store02.Models;
using Store02;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Order_OrderDetailController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository;

        public Order_OrderDetailController(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpPost]
        public IActionResult CreateOrderWithDetails([FromBody] OrderWithDetailsDTO orderWithDetails)
        {
            // Validar si tiene al menos un detalle de la orden
            if (orderWithDetails.OrderDetails == null || orderWithDetails.OrderDetails.Count == 0)
            {
                return BadRequest("Se requiere al menos un detalle para la orden.");
            }

            // Validar CustomerID o SupplierID según el tipo de orden
            if (orderWithDetails.OrderType == "Sale" && orderWithDetails.CustomerID == null)
            {
                return BadRequest("CustomerID es requerido para ventas.");
            }
            if (orderWithDetails.OrderType == "Purchase" && orderWithDetails.SupplierID == null)
            {
                return BadRequest("SupplierID es requerido para compras.");
            }

            // Crear la orden (TotalAmount se inicializa en 0)
            var order = new Order
            {
                CustomerID = orderWithDetails.CustomerID,
                SupplierID = orderWithDetails.SupplierID,
                StatusOrder = orderWithDetails.StatusOrder,
                OrderType = orderWithDetails.OrderType
            };

            int newOrderId = _orderRepository.CreateOrder(order);

            // Crear los detalles de la orden y calcular el total
            decimal totalAmount = 0;
            foreach (var detail in orderWithDetails.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = newOrderId,
                    ProductID = detail.ProductID,
                    Quantity = detail.Quantity,
                    Price = detail.Price
                };

                _orderDetailRepository.CreateOrderDetail(orderDetail);

                // Sumar al total el importe del detalle (Quantity * Price)
                totalAmount += detail.Quantity * detail.Price;
            }

            // Actualizar el TotalAmount en la orden
            _orderRepository.UpdateTotalAmount(newOrderId, totalAmount);

            return Ok("Order and OrderDetails created successfully");
        }

    }







    //// GET: api/<Order_OrderDetailController>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<Order_OrderDetailController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<Order_OrderDetailController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //}

    //// PUT api/<Order_OrderDetailController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<Order_OrderDetailController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}

}
