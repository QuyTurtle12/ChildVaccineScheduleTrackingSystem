using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Package
{
    public class CreateModel : PageModel
    {
        private readonly IPackageService _packageService;

        public CreateModel(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PackagePostDTO Package { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _packageService.CreateAsync(Package);

            return RedirectToPage("./Index");
        }
    }
}
