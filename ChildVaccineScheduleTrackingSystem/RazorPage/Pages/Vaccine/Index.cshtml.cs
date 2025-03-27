using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Vaccine
{
    public class IndexModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public IndexModel(IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }
        public string UserRole { get; set; } = string.Empty;

        public IEnumerable<VaccineGetDto> Vaccine { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Vaccine = await _vaccineService.GetAllAsync();
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();

        }
    }
}
