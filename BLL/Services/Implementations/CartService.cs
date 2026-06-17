using BLL.Common;
using BLL.DTOs.CartDTOs;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CartReadDTO>> GetUserCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);

            var cartDto = new CartReadDTO
            {
                CartId = cart.Id,
                CartItems = cart.CartItems?.Select(ci => new CartItemReadDTO
                {
                    CartItemId = ci.Id,
                    BookId = ci.BookId,
                    BookTitle = ci.Book?.Title ?? string.Empty,
                    PricePerUnit = ci.Book?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList() ?? new List<CartItemReadDTO>()
            };

            cartDto.TotalCartPrice = cartDto.CartItems.Sum(item => item.TotalPrice);

            return ApiResponseHelper.Success(cartDto, "Cart retrieved successfully.", 200);
        }

        public async Task<ApiResponse<bool>> AddOrUpdateItemAsync(string userId, CartItemUploadDTO dto)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == dto.BookId);
            if (book == null)
                return ApiResponseHelper.Fail<bool>("Book not found.", 404);

            if (book.StockQuantity < dto.Quantity)
                return ApiResponseHelper.Fail<bool>($"Only {book.StockQuantity} copies available.", 400);

            var cart = await GetOrCreateCartAsync(userId);
            var existingItem = cart.CartItems?.FirstOrDefault(ci => ci.BookId == dto.BookId);

            if (existingItem != null)
            {
                existingItem.Quantity = dto.Quantity;
                _unitOfWork.CartItemRepository.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity
                };
                _unitOfWork.CartItemRepository.Add(newItem);
            }

            if (await _unitOfWork.SaveChanges() > 0)
                return ApiResponseHelper.Success(true, "Cart updated successfully.", 200);

            return ApiResponseHelper.Fail<bool>("Failed to update cart.", 500);
        }

        public async Task<ApiResponse<bool>> RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.CartItems?.FirstOrDefault(ci => ci.Id == cartItemId);

            if (item == null)
                return ApiResponseHelper.Fail<bool>("Item not found in cart.", 404);

            _unitOfWork.CartItemRepository.Delete(item);

            if (await _unitOfWork.SaveChanges() > 0)
                return ApiResponseHelper.Success(true, "Item removed successfully.", 200);

            return ApiResponseHelper.Fail<bool>("Failed to remove item.", 500);
        }

        public async Task<ApiResponse<bool>> ClearCartAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            if (cart.CartItems == null || !cart.CartItems.Any())
                return ApiResponseHelper.Success(true, "Cart is already empty.", 200);

            foreach (var item in cart.CartItems)
            {
                _unitOfWork.CartItemRepository.Delete(item);
            }

            await _unitOfWork.SaveChanges();
            return ApiResponseHelper.Success(true, "Cart cleared successfully.", 200);
        }

        public async Task<ApiResponse<bool>> SyncGuestCartAsync(string userId, List<CartItemUploadDTO> guestItems)
        {
            if (guestItems == null || !guestItems.Any())
                return ApiResponseHelper.Success(true, "No items to sync.", 200);

            foreach (var item in guestItems)
            {
                await AddOrUpdateItemAsync(userId, item);
            }

            return ApiResponseHelper.Success(true, "Guest cart synced successfully.", 200);
        }

        private async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetWithStringsAsync(
                c => c.UserId == userId,
                "CartItems.Book"
         
            );

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _unitOfWork.CartRepository.Add(cart);
                await _unitOfWork.SaveChanges();
                cart.CartItems = new List<CartItem>();
            }

            return cart;
        }
    }
}
