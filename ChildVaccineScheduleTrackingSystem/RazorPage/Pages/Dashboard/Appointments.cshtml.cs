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

        // Properties to hold filter values for display in the view
        public string? NameSearch { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }

        public AppointmentsModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task OnGetAsync(int index = 1, int pageSize = 10, string? nameSearch = null, DateTimeOffset? fromDateSearch = null, DateTimeOffset? toDateSearch = null)
        {
            // Save filters for display
            NameSearch = nameSearch;
            FromDate = fromDateSearch;
            ToDate = toDateSearch;

            // Call service with date filter values along with other parameters
            AppointmentPage = await _appointmentService.GetAppointments(
                index,
                pageSize,
                idSearch: null,
                userSearch: null,
                nameSearch: nameSearch,
                fromDateSearch: fromDateSearch,
                toDateSearch: toDateSearch,
                statusSearch: 1
            );
        }
    }
}
