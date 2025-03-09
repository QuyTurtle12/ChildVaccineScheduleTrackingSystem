using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Appointments
{
    public class UpdateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public UpdateModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public GetAppointmentDTO Appointment { get; set; } = default!;
        [BindProperty]
        public PutAppointmentDTO UpdatedAppointment { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound("Appointment not found!");
            }

            UpdatedAppointment = new PutAppointmentDTO
            {
                Id = appointment.Id,
                UserId = appointment.UserId, // Ensure this is correctly mapped
                AppointmentDate = appointment.AppointmentDate,
                Name = appointment.Name,
                Status = appointment.Status
            };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UpdatedAppointment == null)
            {
                return NotFound("Updated appointment not found!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _appointmentService.UpdateAppointment(UpdatedAppointment);
            return RedirectToPage("./Index");
        }
    }
}
