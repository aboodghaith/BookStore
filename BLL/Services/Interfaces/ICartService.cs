using BLL.Common;
using BLL.DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartReadDTO>> GetUserCartAsync(string userId);

     
        Task<ApiResponse<bool>> AddOrUpdateItemAsync(string userId, CartItemUploadDTO dto);

        Task<ApiResponse<bool>> RemoveItemFromCartAsync(string userId, int cartItemId);

        Task<ApiResponse<bool>> ClearCartAsync(string userId);


        Task<ApiResponse<bool>> SyncGuestCartAsync(string userId, List<CartItemUploadDTO> guestItems);
    }
}
