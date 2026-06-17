using BLL.Common;
using DAL.Enums;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> PaymentProcess(int orderId , PaymentMethod methode , decimal TotalPrice);

        Task<bool> PaymentRefunded(int orderId);

        Task<bool> PaymentPaid(int orderId);

        Task<ApiResponse<List<Payment>>> GetAllPayments();
    }
}
