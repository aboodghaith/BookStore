using BLL.Common;
using BLL.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {

        Task<ApiResponse<OrderReadDTO>> CreateOrderFromCart(string userId , OrderCreateDTO orderCreateDTO);

        Task<ApiResponse<OrderReadDTO>> CreateOrderNow(string userId, OrderNowCreateDTO orderNowCreateDTO);

        Task<ApiResponse<List<OrderReadDTO>>> GetAllOrdersByUserId(string userId);

        Task<ApiResponse<List<OrderReadDTO>>> GetAllOrders();

        Task<ApiResponse<bool>> ConfirmOrder(int orderId);

        Task<ApiResponse<bool>> ShippedOrder(int orderId);

        Task<ApiResponse<bool>> DeliveredOrder(int orderId);

        Task<ApiResponse<bool>> CancelledOrder(int orderId);
    }
}
