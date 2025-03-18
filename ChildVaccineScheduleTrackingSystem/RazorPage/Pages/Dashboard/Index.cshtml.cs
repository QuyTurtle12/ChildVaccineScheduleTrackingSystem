using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IFeedbackService _feedbackService;
        private readonly IPaymentService _paymentService;
        private readonly IVaccineService _vaccineService;
        private readonly IVaccineRecordService _vaccineRecordService;

        public int AppointmentCount { get; set; }
        public int FeedbackCount { get; set; }
        public int PaymentCount { get; set; }
        public int VaccineCount { get; set; }
        public int VaccineRecordCount { get; set; }

        public IndexModel(
            IAppointmentService appointmentService,
            IFeedbackService feedbackService,
            IPaymentService paymentService,
            IVaccineService vaccineService,
            IVaccineRecordService vaccineRecordService)
        {
            _appointmentService = appointmentService;
            _feedbackService = feedbackService;
            _paymentService = paymentService;
            _vaccineService = vaccineService;
            _vaccineRecordService = vaccineRecordService;
        }

        public async Task OnGetAsync()
        {
            // Get count of appointments
            var appointments = await _appointmentService.GetAllAppointments();
            AppointmentCount = appointments.Count();

            // For feedback and payments we are using paginated endpoints with a large pageSize.
            var feedbackPage = await _feedbackService.GetFeedbacks(1, int.MaxValue, null, null, null);
            FeedbackCount = feedbackPage.TotalCount;

            var paymentPage = await _paymentService.GetPayments(1, int.MaxValue, null, null, null, null, null,1);
            PaymentCount = paymentPage.TotalCount;

            // Get count of vaccines
            var vaccines = await _vaccineService.GetAllAsync();
            VaccineCount = vaccines.Count();

            // Get count of vaccine records
            var vaccineRecords = await _vaccineRecordService.GetAllAsync();
            VaccineRecordCount = vaccineRecords.Count();
        }
    }
}
