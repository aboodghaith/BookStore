using BLL.Common;
using BLL.Services.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFactory _paymentFactory;

        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentFactory paymentFactory , IUnitOfWork unitOfWork)
        {
             _paymentFactory = paymentFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> PaymentPaid(int orderId)
        {
            var Payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.OrderId == orderId);

            if (Payment == null)
                return false; 

            Payment.Status = PaymentStatus.Paid;

            _unitOfWork.PaymentRepository.Update(Payment);

            return await _unitOfWork.SaveChanges() > 0;

        }

        public async Task<bool> PaymentProcess(int orderId, PaymentMethod methode , decimal TotalPrice)
        {
            var PaymentStrategy = _paymentFactory.GetPaymentStrategy(methode);

            bool IsPaymentSuccess = await PaymentStrategy.Process();

            if (IsPaymentSuccess) {

                var Payment = new Payment
                {
                    Amount = TotalPrice,
                    Method = methode,
                    OrderId = orderId,
                    Status = methode == PaymentMethod.Cash ? PaymentStatus.Pending : PaymentStatus.Paid
                };

                _unitOfWork.PaymentRepository.Add(Payment);
                return true;
            } 


            return false;
        }

        public async Task<bool> PaymentRefunded(int orderId)
        {
            var Order = await _unitOfWork.OrderRepository.GetWithStringsAsync(o => o.Id == orderId, "OrderItems.Book");

            if (Order == null)
                return false;

            var itemsList = Order.OrderItems.ToList();

            foreach (var item in itemsList)
            {
                if (item.Book == null)
                    return false;

                item.Book.StockQuantity += item.Quantity;
                _unitOfWork.BookRepository.Update(item.Book);

                _unitOfWork.OrderItemRepository.Delete(item);
            }

            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.OrderId == orderId);
            if (payment != null)
            {
                payment.Status = PaymentStatus.Refunded;
                _unitOfWork.PaymentRepository.Update(payment);
            }

            Order.Status = OrderStatus.Cancelled;
                _unitOfWork.OrderRepository.Update(Order);


            return await _unitOfWork.SaveChanges() > 0;
        }


        public async Task<ApiResponse<List<Payment>>> GetAllPayments()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();

            if (payments == null || !payments.Any())
                return ApiResponseHelper.Fail<List<Payment>>("No payments found.", 404);

            return ApiResponseHelper.Success(payments.ToList(), "All payments retrieved successfully.", 200);
        }
    }
}
