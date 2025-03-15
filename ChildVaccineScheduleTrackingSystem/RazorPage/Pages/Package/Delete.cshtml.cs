using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Package
{
    public class DeleteModel : PageModel
    {
        private readonly IPackageService _packageService;

        public DeleteModel(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [BindProperty]
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
