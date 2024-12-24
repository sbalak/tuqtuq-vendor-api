using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Infrastructure;

namespace Vendor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private IOrderService _order;

        public OrderController(IOrderService order)
        {
            _order = order;
        }

        [HttpGet("List")]
        public async Task<List<OrderModel>> List(int restaurantId, int? page = 1, int? pageSize = 10)
        {
            var orders = await _order.GetOrders(restaurantId, page, pageSize);
            return orders;
        }

        [HttpGet("Details")]
        public async Task<OrderModel> Details(int orderId)
        {
            var order = await _order.GetOrder(orderId);
            return order;
        }

        [HttpGet("Accept")]
        public async Task Accept(int orderId)
        {
            await _order.Accept(orderId);
        }

        [HttpGet("Reject")]
        public async Task Reject(int orderId)
        {
            await _order.Reject(orderId);
        }

        [HttpGet("Complete")]
        public async Task Complete(int orderId)
        {
            await _order.Complete(orderId);
        }
    }
}
