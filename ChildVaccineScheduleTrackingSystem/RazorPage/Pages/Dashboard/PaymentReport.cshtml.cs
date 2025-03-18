using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RazorPage.Pages.Dashboard
{
    public class PaymentReportModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        // These JSON strings will hold the chart data for consumption in JavaScript.
        public string PaymentCountByMethodJson { get; set; } = "";
        public string PaymentAmountByMethodJson { get; set; } = "";

        public PaymentReportModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task OnGetAsync()
        {
            // Retrieve all payments
            var payments = await _paymentService.GetAllPayments();

            // Group payments by PaymentMethod and count them.
            var countByMethod = payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    Count = g.Count()
                })
                .ToList();

            PaymentCountByMethodJson = JsonConvert.SerializeObject(countByMethod);

            // Group payments by PaymentMethod and sum their Amounts.
            var amountByMethod = payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    TotalAmount = g.Sum(p => p.Amount)
                })
                .ToList();

            PaymentAmountByMethodJson = JsonConvert.SerializeObject(amountByMethod);
        }
    }
}
