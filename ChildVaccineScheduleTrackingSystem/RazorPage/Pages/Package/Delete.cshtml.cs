using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Services;

namespace RazorPage.Pages.Package
{
    public class DeleteModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IJwtTokenService _jwtTokenService;

        public DeleteModel(IPackageService packageService, IJwtTokenService jwtTokenService)
        {
            _packageService = packageService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public PackageGetDTO Package { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff")
            {
                return Forbid();
            }

            var package = await _packageService.GetByIdAsync(id);

            if (package == null)
            {
                return NotFound();
            }
            else
            {
                Package = package;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if(await _packageService.DeleteAsync(id))
            {
                return RedirectToPage("./Index");
            }


            return BadRequest("Delete failed!");
        }
    }
}
