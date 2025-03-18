using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RazorPage.Pages.Dashboard
{
    public class ChartModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        // This JSON string will hold the chart data for consumption in JavaScript.
        public string ChartDataJson { get; set; } = "";

        public ChartModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task OnGetAsync()
        {
            // Retrieve all appointments (adjust this if you want to limit the range)
            var appointments = await _appointmentService.GetAllAppointments();

            // Group appointments by date (only date portion) and count them.
            var groupedData = appointments
                .GroupBy(a => a.AppointmentDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                });

            // Serialize the grouped data to JSON
            ChartDataJson = JsonConvert.SerializeObject(groupedData);
        }
    }
}
