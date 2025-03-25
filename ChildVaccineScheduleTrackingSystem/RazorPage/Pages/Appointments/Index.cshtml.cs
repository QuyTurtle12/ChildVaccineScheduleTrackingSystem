using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Appointments
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        private const string STAFF_ROLE = "Staff";
        private const string CUSTOMER_ROLE = "Customer";

        public PaginatedList<GetAppointmentDTO> Appointments { get; set; }
        public IndexModel(IAppointmentService appointmentService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }
        // Search Filters
        [BindProperty(SupportsGet = true)]
        public string? UserSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? NameSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? FromDateSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? ToDateSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? StatusSearch { get; set; }


        // GET: Get and Search Categories
        //public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? userSearch = null, string? nameSearch = null, DateTimeOffset? fromDateSearch = null, DateTimeOffset? toDateSearch = null, int? statusSearch = null)
        //{
        //    var jwtToken = HttpContext.Session.GetString("jwt_token");
        //    var userRole = _jwtTokenService.GetRole(jwtToken);
        //    ViewData["JwtToken"] = jwtToken;
        //    ViewData["UserRole"] = userRole;
        //    Console.WriteLine($"JWT Token: {jwtToken}");
        //    // Fetch paginated search categories
        //    Appointments = await _appointmentService.GetAppointments(pageNumber, pageSize, null, userSearch, nameSearch, fromDateSearch, toDateSearch, statusSearch);
        //}

        // GET: Get and Search Categories
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 3, string? userSearch = null, string? userIdSearch = null, string? nameSearch = null, DateTimeOffset? fromDateSearch = null, DateTimeOffset? toDateSearch = null, int? statusSearch = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            var userRole = _jwtTokenService.GetRole(jwtToken);

            if (userRole == null) return Unauthorized();

            if (userRole != STAFF_ROLE && userRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            ViewData["JwtToken"] = jwtToken;
            ViewData["UserRole"] = userRole;
            Console.WriteLine($"JWT Token: {jwtToken}");
            // Fetch paginated search categories

            if (userRole == CUSTOMER_ROLE) // Customer function
            {
                userIdSearch = _jwtTokenService.GetId(jwtToken);
                Appointments = await _appointmentService.GetAppointments(pageNumber, pageSize, null, userSearch, userIdSearch, nameSearch, fromDateSearch, toDateSearch, statusSearch);
            }
            else if (userRole == STAFF_ROLE) // Staff function
            {
                Appointments = await _appointmentService.GetAppointments(pageNumber, pageSize, null, userSearch, userIdSearch, nameSearch, fromDateSearch, toDateSearch, statusSearch);
            }
            
            return Page();
        }
    }
}
