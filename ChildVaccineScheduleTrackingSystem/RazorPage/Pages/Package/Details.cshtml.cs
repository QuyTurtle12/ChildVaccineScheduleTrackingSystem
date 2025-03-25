using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Package
{
    public class DetailsModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public DetailsModel(IPackageService packageService, IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _packageService = packageService;
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        public PackageGetDTO Package { get; set; } = default!;
        public IEnumerable<VaccineGetDto> Vaccines { get; set; } = default!;
        public string UserRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var package = await _packageService.GetByIdAsync(id);
            
            if (package == null)
            {
                return NotFound();
            }
            else
            {
                Package = package;
                var vaccines = await _vaccineService.GetVaccineByPackageId(id);
                Vaccines = vaccines;
            }
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            UserRole = loggedInUserRole.ToLower();

            
            return Page();
        }
    }
}
