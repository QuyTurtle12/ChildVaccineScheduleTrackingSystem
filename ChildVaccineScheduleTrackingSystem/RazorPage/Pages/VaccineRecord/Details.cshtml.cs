using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.VaccineRecord
{
    public class DetailsModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IJwtTokenService _jwtTokenService;
        public DetailsModel(IVaccineRecordService vaccineRecordService, IJwtTokenService jwtTokenService)
        {
            _vaccineRecordService = vaccineRecordService;
            _jwtTokenService = jwtTokenService;
        }

        public GetVaccineRecordDto VaccineRecord { get; set; } = default!;
        public string UserRole { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff" && loggedInUserRole.ToLower() != "customer")
            {
                return Forbid();
            }

            var vaccineRecord = await _vaccineRecordService.GetByIdAsync(id);
            if (vaccineRecord == null)
            {
                return NotFound();
            }
            else
            {
                VaccineRecord = vaccineRecord;
            }
            
            
            return Page();
        }
    }
}
