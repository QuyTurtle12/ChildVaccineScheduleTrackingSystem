using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Dashboard
{
    public class AppointmentsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public PaginatedList<GetAppointmentDTO> AppointmentPage { get; set; }

        public AppointmentsModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task OnGetAsync(int index = 1, int pageSize = 10, string? nameSearch = null)
        {
            // You can add additional search parameters as needed.
            // Here, other parameters are passed as null.
            AppointmentPage = await _appointmentService.GetAppointments(index, pageSize, null, null, nameSearch, null, null, null);
        }
    }
}
