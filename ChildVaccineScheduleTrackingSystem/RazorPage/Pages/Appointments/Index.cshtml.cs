using AutoMapper;
using BusinessLogic.DTOs.AppointmentDTO;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public PaginatedList<GetAppointmentDTO> Appointments { get; set; }
        public IndexModel(IAppointmentService appointmentService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }


        // GET: Get and Search Categories
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? userSearch = null, string? nameSearch = null, DateTimeOffset? fromDateSearch = null, DateTimeOffset? toDateSearch = null, int? statusSearch = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            var userRole = _jwtTokenService.GetRole(jwtToken);
            ViewData["JwtToken"] = jwtToken;
            ViewData["UserRole"] = userRole;
            Console.WriteLine($"JWT Token: {jwtToken}");
            // Fetch paginated search categories
            Appointments = await _appointmentService.GetAppointments(pageNumber, pageSize, null, userSearch, nameSearch, fromDateSearch, toDateSearch, statusSearch);
        }
    }
}
