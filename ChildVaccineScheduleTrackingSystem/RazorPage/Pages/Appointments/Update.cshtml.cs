using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace RazorPage.Pages.Appointments
{
    [Authorize(Roles = "Staff, Customer")]
    public class UpdateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public UpdateModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

       
        public GetAppointmentDTO Appointment { get; set; } = default!;
        [BindProperty]
        public PutAppointmentDTO UpdatedAppointment { get; set; } = default!;
        public List<SelectListItem> StatusList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ActionType { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound("Không tìm thấy cuộc hẹn!");
            }
            if (!string.IsNullOrEmpty(ActionType) && ActionType == "Cancel")
            {
                // Handle the case where the user is canceling the appointment
                TempData["Message"] = "Bạn có chắc chắn muốn hủy cuộc hẹn này không?";
            }

            Appointment = appointment;

            // Populate status dropdown list
            StatusList = Enum.GetValues(typeof(EnumAppointment))
                .Cast<EnumAppointment>()
                .Select(e => new SelectListItem
                {
                    //Value = ((int)e).ToString(),
                    //Text = e.ToString()
                    Value = ((int)e).ToString(),
                     Text = e.GetType() // Get the type of EnumAppointment
                     .GetField(e.ToString()) // Get the specific enum field
                     ?.GetCustomAttribute<DisplayAttribute>()?
                     .Name ?? e.ToString() // Use Display Name if available, otherwise default enum name
                }).ToList();

            UpdatedAppointment = new PutAppointmentDTO
            {
                Id = appointment.Id,
                UserId = appointment.UserId, // Ensure this is correctly mapped
               // UserId = Guid.Parse(userIdClaim.Value), Use when have token
                AppointmentDate = appointment.AppointmentDate,
                Name = appointment.Name,
                Status = appointment.Status,
                CreatedBy = appointment.CreatedBy,
                CreatedTime = appointment.CreatedTime,
                LastUpdatedBy = appointment.LastUpdatedBy,
                LastUpdatedTime = appointment.LastUpdatedTime,
            };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UpdatedAppointment == null)
            {
                return NotFound("Không tìm thấy cuộc hẹn đã cập nhật!");
            }
            // Retrieve the existing appointment before validation
            Appointment = await _appointmentService.GetAppointmentById(UpdatedAppointment.Id);
            if (Appointment == null)
            {
                return NotFound("Không tìm thấy cuộc hẹn!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //if (Appointment.Status == EnumAppointment.Completed && UpdatedAppointment.Status == EnumAppointment.Canceled)
            //{
            //    return NotFound("Cannot cancel an appointment that is already completed!");
            //}
            if (Appointment.Status == EnumAppointment.Completed && UpdatedAppointment.Status == EnumAppointment.Canceled)
            {
                ModelState.AddModelError(string.Empty, "Không thể hủy cuộc hẹn đã hoàn tất!");
                return Page();
            }

            await _appointmentService.UpdateAppointment(UpdatedAppointment);
            return RedirectToPage("./Index");
        }
    }
}
