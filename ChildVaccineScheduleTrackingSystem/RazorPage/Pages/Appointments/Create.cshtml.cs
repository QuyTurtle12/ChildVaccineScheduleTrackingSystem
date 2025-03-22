using BusinessLogic.DTOs;
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
        private readonly IPackageService _packageService;


        [BindProperty]
        public PostAppointmentDTO Appointment { get; set; }

        public string UserRole { get; set; }

        public List<PackageGetDTO> Packages { get; set; } // Store available packages

        [BindProperty]
        public List<Guid> PackageIds { get; set; }

        public CreateModel(IAppointmentService appointmentService, IUserService userService, IJwtTokenService jwtTokenService, IPackageService packageService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _packageService = packageService;
            Packages = new List<PackageGetDTO>();
        }



        // GET: Create Appointment
        public async Task<IActionResult> OnGetAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            UserRole = _jwtTokenService.GetRole(jwtToken);

            /*Packages = (await _packageService.GetAllAsync()).ToList();*/ // Load available packages
            var packages = await _packageService.GetAllAsync(); // Fetch packages
            if (packages != null)
            {
                Packages = packages.ToList();
            }
            else
            {
                Packages = new List<PackageGetDTO>(); // Ensure it's not null
            }

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
                Appointment.CustomerPhoneNumber = string.Empty; // Ensure only their own booking
                Appointment.UserId = Guid.Parse(userId);
            }
            else if (UserRole == "Staff")
            {

                if (string.IsNullOrEmpty(Appointment.CustomerPhoneNumber))
                {
                    ModelState.AddModelError("Appointment.CustomerPhoneNumber", "Customer phone number is for booking.");
                    return Page();
                }
                // Find user by phone number
                var customer = await _userService.GetUserByPhoneNumber(Appointment.CustomerPhoneNumber);
                if (customer == null)
                {
                    ModelState.AddModelError("Appointment.CustomerPhoneNumber", "Customer not found.");
                    return Page();
                }

                Appointment.UserId = Guid.Parse(customer.Id); // Assign the customer ID
            }

            if (Appointment.AppointmentDate <= DateTimeOffset.Now.AddMinutes(1))
            {
                ModelState.AddModelError("Appointment.AppointmentDate", "Date can not be a date in past or presence.");
                return Page();
            }


            if (Appointment.PackageIds == null || !Appointment.PackageIds.Any())
            {
                ModelState.AddModelError("Appointment.PackageIds", "At least one package must be selected.");
                return Page();
            }

            await _appointmentService.CreateAppointment(Appointment);
            return RedirectToPage("./Index");
        }
    }
}
