using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace RazorPage.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        [BindProperty]
        public PostAppointmentDTO Appointment { get; set; }

        public string UserRole { get; set; }

        public CreateModel(IAppointmentService appointmentService, IUserService userService, IJwtTokenService jwtTokenService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }



        // GET: Create Appointment
        public IActionResult OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            UserRole = _jwtTokenService.GetRole(jwtToken);
            return Page();
        }




        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            UserRole = _jwtTokenService.GetRole(jwtToken);
            var userId = _jwtTokenService.GetId(jwtToken);

            if (UserRole == "Customer")
            {
                // Customers book for themselves
                Appointment.CustomerPhoneNumber = null; // Ensure only their own booking
                Appointment.UserId = Guid.Parse(userId);
            }
            else if (UserRole == "Staff")
            {

                if (string.IsNullOrEmpty(Appointment.CustomerPhoneNumber))
                {
                    ModelState.AddModelError("", "Customer phone number is for booking.");
                    return Page();
                }
                // Find user by phone number
                var customer = await _userService.GetUserByPhoneNumber(Appointment.CustomerPhoneNumber);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer not found.");
                    return Page();
                }

                Appointment.UserId = Guid.Parse(customer.Id); // Assign the customer ID
            }

            if (Appointment.AppointmentDate <= DateTimeOffset.Now)
            {
                ModelState.AddModelError("", "Date can not be a date in past or presence.");
                return Page();
            }

            await _appointmentService.CreateAppointment(Appointment);
            return RedirectToPage("./Index");
        }
    }
}
