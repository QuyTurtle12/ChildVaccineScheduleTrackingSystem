using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.VaccineRecord
{
    public class IndexModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IJwtTokenService _jwtTokenService;

        public IndexModel(IVaccineRecordService vaccineRecordService, IJwtTokenService jwtTokenService)
        {
            _vaccineRecordService = vaccineRecordService;
            _jwtTokenService = jwtTokenService;
        }
        public string UserRole { get; set; } = string.Empty;
        public IEnumerable<GetVaccineRecordDto> VaccineRecord { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            string loggedInUserId = _jwtTokenService.GetId(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff" && loggedInUserRole.ToLower() != "customer")
            {
                return Forbid();
            }
            UserRole = loggedInUserRole.ToLower();

            if (UserRole == "staff") VaccineRecord = await _vaccineRecordService.GetAllAsync();

            if(UserRole == "customer") VaccineRecord = await _vaccineRecordService.GetByUserId(Guid.Parse(loggedInUserId));

            return Page();
        }
    }
}
