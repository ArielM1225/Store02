using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Store02.Models;
using Store02;
using Microsoft.Data.SqlClient;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store02.Controllers
{
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
}
