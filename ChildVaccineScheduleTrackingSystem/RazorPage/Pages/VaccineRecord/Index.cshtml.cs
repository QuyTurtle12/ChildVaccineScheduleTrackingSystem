using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

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

        public async Task OnGetAsync()
        {
            VaccineRecord = await _vaccineRecordService.GetAllAsync();
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();
        }
    }
}
