using BLL.DTOs.OrderDTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateFromCart")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateFromCart(OrderCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var response = await _orderService.CreateOrderFromCart(userId, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateNow")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateNow(OrderNowCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var response = await _orderService.CreateOrderNow(userId, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Confirm/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var response = await _orderService.ConfirmOrder(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Ship/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShipOrder(int id)
        {
            var response = await _orderService.ShippedOrder(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Deliver/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeliverOrder(int id)
        {
            var response = await _orderService.DeliveredOrder(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("Cancel/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var response = await _orderService.CancelledOrder(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _orderService.GetAllOrders();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("MyOrders")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var response = await _orderService.GetAllOrdersByUserId(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}