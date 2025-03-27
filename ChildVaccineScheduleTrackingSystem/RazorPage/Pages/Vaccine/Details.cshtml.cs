using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Vaccine
{
    public class DetailsModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public DetailsModel(IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        public VaccineGetDto Vaccine { get; set; } = default!;
        public string UserRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vaccine = await _vaccineService.GetByIdAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            else
            {
                Vaccine = vaccine;
            }

            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();
            return Page();
        }
    }
}
