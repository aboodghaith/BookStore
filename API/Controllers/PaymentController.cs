using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPut("MarkAsPaid/{orderId}")]
        public async Task<IActionResult> MarkAsPaid(int orderId)
        {
            var result = await _paymentService.PaymentPaid(orderId);

            if (result)
                return Ok(new { Message = "Payment status updated to Paid successfully." });

            return BadRequest(new { Message = "Failed to update payment status or payment record not found." });
        }

        [HttpPost("Refund/{orderId}")]
        public async Task<IActionResult> RefundOrder(int orderId)
        {
            var result = await _paymentService.PaymentRefunded(orderId);

            if (result)
                return Ok(new { Message = "Order and payment completely refunded and stock restored." });

            return BadRequest(new { Message = "Refund failed. Make sure order exists and can be refunded." });
        }


        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentService.GetAllPayments();
            return StatusCode(response.StatusCode, response);
        }
    }
}
