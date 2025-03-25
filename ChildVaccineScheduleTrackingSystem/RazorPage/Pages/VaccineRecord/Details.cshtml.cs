using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
            var vaccinerecord = await _vaccineRecordService.GetByIdAsync(id);
            if (vaccinerecord == null)
            {
                return NotFound();
            }
            else
            {
                VaccineRecord = vaccinerecord;
            }
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();
            return Page();
        }
    }
}
