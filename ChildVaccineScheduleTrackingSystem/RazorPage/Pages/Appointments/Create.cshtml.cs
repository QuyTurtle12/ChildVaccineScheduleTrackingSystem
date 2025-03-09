using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RazorPage.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;


        [BindProperty]
        public PostAppointmentDTO Appointment { get; set; }

        public CreateModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: Create Appointment
        public IActionResult OnGet()
        {
            return Page();
        }


        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Appointment.CreatedBy = "system"; // Will use token
            Appointment.CreatedTime = DateTime.Now;
            Appointment.LastUpdatedBy = "system"; // Will use token
            Appointment.LastUpdatedTime = DateTime.Now;

            await _appointmentService.CreateAppointment(Appointment);
            return Page();
        }
    }
}
