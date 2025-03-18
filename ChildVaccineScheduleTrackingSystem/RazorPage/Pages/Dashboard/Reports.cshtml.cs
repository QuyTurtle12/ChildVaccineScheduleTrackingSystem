using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RazorPage.Pages.Dashboard
{
    public class ReportsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;
        private readonly IFeedbackService _feedbackService;

        public ReportsModel(
            IAppointmentService appointmentService,
            IPaymentService paymentService,
            IFeedbackService feedbackService)
        {
            _appointmentService = appointmentService;
            _paymentService = paymentService;
            _feedbackService = feedbackService;
        }

        // Date range filter (supports binding from query string)
        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? ToDate { get; set; }

        // Summary counts
        public int TotalAppointments { get; set; }
        public int TotalPayments { get; set; }
        public int TotalFeedbacks { get; set; }

        // JSON strings for Chart.js charts
        public string AppointmentTrendJson { get; set; }
        public string PaymentCountByMethodJson { get; set; }
        public string PaymentAmountByMethodJson { get; set; }
        public string FeedbackCountByRatingJson { get; set; }

        public async Task OnGetAsync()
        {
            // Retrieve all data from services
            var appointments = await _appointmentService.GetAllAppointments();
            var payments = await _paymentService.GetAllPayments();
            var feedbackPage = await _feedbackService.GetFeedbacks(1, int.MaxValue, null, null, null);
            var feedbacks = feedbackPage.Items;

            // Apply date filters (if provided) based on the entity's date properties.
            // Adjust property names as needed.
            if (FromDate.HasValue)
            {
                appointments = appointments.Where(a => a.AppointmentDate.Date >= FromDate.Value.Date);
                payments = payments.Where(p => p.CreatedTime.Date >= FromDate.Value.Date);
                feedbacks = feedbacks.Where(f => f.CreatedTime.Date >= FromDate.Value.Date).ToList();
            }
            if (ToDate.HasValue)
            {
                appointments = appointments.Where(a => a.AppointmentDate.Date <= ToDate.Value.Date);
                payments = payments.Where(p => p.CreatedTime.Date <= ToDate.Value.Date);
                feedbacks = feedbacks.Where(f => f.CreatedTime.Date <= ToDate.Value.Date).ToList();
            }

            // Calculate summary counts
            TotalAppointments = appointments.Count();
            TotalPayments = payments.Count();
            TotalFeedbacks = feedbacks.Count();

            // Chart 1: Appointment Trend (Appointments per day)
            var appointmentTrend = appointments
                .GroupBy(a => a.AppointmentDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                }).ToList();
            AppointmentTrendJson = JsonConvert.SerializeObject(appointmentTrend);

            // Chart 2: Payment Report – Count by Payment Method
            var paymentCountByMethod = payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new {
                    PaymentMethod = g.Key,
                    Count = g.Count()
                }).ToList();
            PaymentCountByMethodJson = JsonConvert.SerializeObject(paymentCountByMethod);

            // Chart 3: Payment Report – Total Amount by Payment Method
            var paymentAmountByMethod = payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new {
                    PaymentMethod = g.Key,
                    TotalAmount = g.Sum(p => p.Amount)
                }).ToList();
            PaymentAmountByMethodJson = JsonConvert.SerializeObject(paymentAmountByMethod);

            // Chart 4: Feedback Report – Count by Rating
            var feedbackCountByRating = feedbacks
                .GroupBy(f => f.Rating)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Rating = g.Key,
                    Count = g.Count()
                }).ToList();
            FeedbackCountByRatingJson = JsonConvert.SerializeObject(feedbackCountByRating);
        }
    }
}

