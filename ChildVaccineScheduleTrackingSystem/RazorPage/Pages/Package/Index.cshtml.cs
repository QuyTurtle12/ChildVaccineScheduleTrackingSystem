using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.Package
{
    public class IndexModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IJwtTokenService _jwtTokenService;

        public IndexModel(IPackageService packageService, IJwtTokenService jwtTokenService)
        {
            _packageService = packageService;
            _jwtTokenService = jwtTokenService;
        }

        public IEnumerable<PackageGetDTO> Package { get; set; } = default!;
        public string UserRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            Package = await _packageService.GetAllAsync();
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            if (!string.IsNullOrWhiteSpace(loggedInUserRole))
            {
                UserRole = loggedInUserRole.ToLower();
            }
            
            return Page();
        }
    }
}
