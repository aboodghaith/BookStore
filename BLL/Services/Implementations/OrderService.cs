using BLL.Common;
using BLL.DTOs.OrderDTOs;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public OrderService(IUnitOfWork unitOfWork , IPaymentService paymentService) { 
            
            _unitOfWork = unitOfWork; 
            _paymentService = paymentService;
        
        }
        public async Task<ApiResponse<OrderReadDTO>> CreateOrderFromCart(string userId, OrderCreateDTO orderCreateDTO)
        {
            var Cart = await _unitOfWork.CartRepository.GetWithStringsAsync(
             c => c.UserId == userId,
             "CartItems.Book"
         );
            if (Cart == null)
                return ApiResponseHelper.Fail<OrderReadDTO>("Cart not found", 404);

            if (Cart.CartItems == null || Cart.CartItems.Count == 0)
                return ApiResponseHelper.Fail<OrderReadDTO>("Cart is Empty");

            

            foreach (var item in Cart.CartItems!)
            {
                if (item.Book == null)
                    return ApiResponseHelper.Fail<OrderReadDTO>("One of the books in your cart no longer exists.", 404);
                // if 1 stock so the condation is false 
                if (item.Quantity > item.Book.StockQuantity)
                        return ApiResponseHelper.Fail<OrderReadDTO>($"Only {item.Book.StockQuantity} copies available.", 400);

            }
            var TotalPriceOnCart = Cart.CartItems.Sum(c => c.Quantity * c.Book.Price);
            using var Transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Order = new Order
                {
                    UserId = userId,
                    OrderItems = new List<OrderItem>(),
                    Status = OrderStatus.Pending,
                    TotalPrice = TotalPriceOnCart,

                };

                foreach (var item in Cart.CartItems)
                {
                    var OrderItem = new OrderItem
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        Author = item.Book.Author,
                        Title = item.Book.Title,
                        Price = item.Book.Price
                    };

                    Order.OrderItems.Add(OrderItem);
                    item.Book.StockQuantity -= item.Quantity;
                    _unitOfWork.BookRepository.Update(item.Book);
                    _unitOfWork.CartItemRepository.Delete(item);
                }
                _unitOfWork.OrderRepository.Add(Order);

                if (await _unitOfWork.SaveChanges() <= 0)
                {
                    throw new Exception("Failed to save the order record.");
                }

                var IsProcessPayment = await _paymentService.PaymentProcess(Order.Id, orderCreateDTO.Method, TotalPriceOnCart);


                if (!IsProcessPayment)
                {
                    throw new Exception("Payment failed.");
                }

         
                
                    if (orderCreateDTO.Method != PaymentMethod.Cash)
                    {
                        Order.Status = OrderStatus.Confirmed;
                        _unitOfWork.OrderRepository.Update(Order);

                    }
                

                if (await _unitOfWork.SaveChanges() <= 0)
                {
                    throw new Exception("Failed to finalize order and payment updates.");
                }


                await Transaction.CommitAsync();

                var result = new OrderReadDTO
                {
                    OrderId = Order.Id,
                    TotalPrice = Order.TotalPrice,
                    Status = Order.Status.ToString(),
                    CreatedAt = Order.CreatedAt
                };

                return ApiResponseHelper.Success(result, "Order placed successfully from cart.", 201);
            }
            catch (Exception ex) {

                await Transaction.RollbackAsync();
                return ApiResponseHelper.Fail<OrderReadDTO>($"Order failed: {ex.Message}", 500);
            } 
        }


        #region Common 
        private async Task<ApiResponse<bool>> UpdateStatusHelperAsync(int orderId, OrderStatus CurrentStatus, OrderStatus newStatus, string failMessage)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(o => o.Id == orderId);

            if (order == null)
                return ApiResponseHelper.Fail<bool>("Order Not Found", 404);

            if (order.Status != CurrentStatus)
                return ApiResponseHelper.Fail<bool>($"Action invalid. Order is currently {order.Status}.", 400);

            order.Status = newStatus;
            _unitOfWork.OrderRepository.Update(order);

            if (await _unitOfWork.SaveChanges() > 0)
            {
                return ApiResponseHelper.Success(true, $"Order updated to {newStatus} successfully.", 200);
            }

            return ApiResponseHelper.Fail<bool>(failMessage, 500);
        }

        #endregion Common 


        public async Task<ApiResponse<bool>> ConfirmOrder(int orderId)
        {
            return await UpdateStatusHelperAsync(orderId, OrderStatus.Pending, OrderStatus.Confirmed, "Failed To Confirm Order");
        }

        public async Task<ApiResponse<bool>> ShippedOrder(int orderId)
        {
            return await UpdateStatusHelperAsync(orderId, OrderStatus.Confirmed, OrderStatus.Shipped, "Failed To Ship Order");
        }

        public async Task<ApiResponse<bool>> DeliveredOrder(int orderId)
        {
            return await UpdateStatusHelperAsync(orderId, OrderStatus.Shipped, OrderStatus.Delivered, "Failed To Deliver Order");
        }

        public async Task<ApiResponse<bool>> CancelledOrder(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetWithStringsAsync(o => o.Id == orderId, "OrderItems.Book");

            if (order == null)
                return ApiResponseHelper.Fail<bool>("Order Not Found", 404);

            if (order.Status != OrderStatus.Pending)
                return ApiResponseHelper.Fail<bool>("Can only cancel pending orders.", 400);

           
            // return stock quntity to book
            
                foreach (var item in order.OrderItems!)
                {
                    if (item.Book != null)
                    {
                        item.Book.StockQuantity += item.Quantity;
                        _unitOfWork.BookRepository.Update(item.Book);
                    }
                }

                order.Status = OrderStatus.Cancelled;
                _unitOfWork.OrderRepository.Update(order);

                await _unitOfWork.SaveChanges();
             

                return ApiResponseHelper.Success(true, "Order cancelled and stock restored.", 200);
           
        }


        public async Task<ApiResponse<OrderReadDTO>> CreateOrderNow(string userId, OrderNowCreateDTO orderNowCreateDTO)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == orderNowCreateDTO.BookId);

            if (book == null)
                return ApiResponseHelper.Fail<OrderReadDTO>("Book not found.", 404);

            if (orderNowCreateDTO.Quantity > book.StockQuantity)
                return ApiResponseHelper.Fail<OrderReadDTO>($"Only {book.StockQuantity} copies available.", 400);

            var TotalPriceNow = orderNowCreateDTO.Quantity * book.Price;

            using var Transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Order = new Order
                {
                    UserId = userId,
                    OrderItems = new List<OrderItem>(),
                    Status = OrderStatus.Pending,
                    TotalPrice = TotalPriceNow,
                    
                };

                var OrderItem = new OrderItem
                {
                    BookId = book.Id,
                    Quantity = orderNowCreateDTO.Quantity,
                    Author = book.Author ?? string.Empty,
                    Title = book.Title,
                    Price = book.Price
                };

                Order.OrderItems.Add(OrderItem);

              
                book.StockQuantity -= orderNowCreateDTO.Quantity;
                _unitOfWork.BookRepository.Update(book);

                _unitOfWork.OrderRepository.Add(Order);

                if (await _unitOfWork.SaveChanges() <= 0)
                {
                    throw new Exception("Failed to save the instant order record.");
                }

                var IsProcessPayment = await _paymentService.PaymentProcess(Order.Id, orderNowCreateDTO.Method, TotalPriceNow);



                if (!IsProcessPayment)
                {
                    throw new Exception("Payment failed.");
                }



                if (orderNowCreateDTO.Method != PaymentMethod.Cash)
                {
                    Order.Status = OrderStatus.Confirmed;
                    _unitOfWork.OrderRepository.Update(Order);

                }

                if (await _unitOfWork.SaveChanges() <= 0)
                {
                    throw new Exception("Failed to finalize order and payment updates.");
                }

                await Transaction.CommitAsync();

                var result = new OrderReadDTO
                {
                    OrderId = Order.Id,
                    TotalPrice = Order.TotalPrice,
                    Status = Order.Status.ToString(),
                    CreatedAt = Order.CreatedAt
                };

                return ApiResponseHelper.Success(result, "Instant order placed successfully.", 201);
            }
            catch (Exception ex)
            {
                await Transaction.RollbackAsync();
                return ApiResponseHelper.Fail<OrderReadDTO>($"Instant order failed: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<OrderReadDTO>>> GetAllOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
                return ApiResponseHelper.Fail<List<OrderReadDTO>>("No orders found.", 404);

            var result = orders.Select(o => new OrderReadDTO
            {
                OrderId = o.Id,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList();

            return ApiResponseHelper.Success(result, "All orders retrieved successfully.", 200);
        }

        public async Task<ApiResponse<List<OrderReadDTO>>> GetAllOrdersByUserId(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(o => o.UserId == userId);

            if (orders == null || !orders.Any())
                return ApiResponseHelper.Fail<List<OrderReadDTO>>("No orders found for this user.", 404);

            var result = orders.Select(o => new OrderReadDTO
            {
                OrderId = o.Id,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList();

            return ApiResponseHelper.Success(result, "User orders retrieved successfully.", 200);
        }
    }
}
