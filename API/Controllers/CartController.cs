using BLL.DTOs.CartDTOs;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            var response = await _cartService.GetUserCartAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("add-or-update")]
        public async Task<IActionResult> AddOrUpdateItem(CartItemUploadDTO dto)
        {
            var userId = GetCurrentUserId();
            var response = await _cartService.AddOrUpdateItemAsync(userId, dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetCurrentUserId();
            var response = await _cartService.RemoveItemFromCartAsync(userId, cartItemId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            var response = await _cartService.ClearCartAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncCart( List<CartItemUploadDTO> guestItems)
        {
            var userId = GetCurrentUserId();
            var response = await _cartService.SyncGuestCartAsync(userId, guestItems);
            return StatusCode(response.StatusCode, response);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
        }
    }
}
