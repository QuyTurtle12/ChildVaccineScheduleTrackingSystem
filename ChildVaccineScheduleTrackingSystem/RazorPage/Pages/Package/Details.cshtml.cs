using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Package
{
    public class DetailsModel : PageModel
    {
        private readonly IPackageService _packageService;

        public DetailsModel(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public PackageGetDTO Package { get; set; } = default!;

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
            }
            return Page();
        }
    }
}
