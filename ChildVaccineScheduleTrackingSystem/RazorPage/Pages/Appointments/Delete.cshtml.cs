using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Appointments
{
    public class DeleteModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public DeleteModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public GetAppointmentDTO Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _appointmentService.DeleteAppointment(id);
            return RedirectToPage("./Index");
        }
    }
}
