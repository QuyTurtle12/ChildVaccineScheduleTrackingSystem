using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RazorPage.Pages.Package
{
    public class EditModel : PageModel
    {
        private readonly IPackageService _packageService;

        public EditModel(IPackageService packageService)
        {
            _packageService = packageService;
        }
        public PackageGetDTO PackageGetDto { get; set; } = default!;

        [BindProperty]
        public PackagePutDTO Package { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            
            var package =  await _packageService.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            else
            {
                PackageGetDto = package;
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _packageService.UpdateAsync(PackageGetDto.Id, Package);

            return RedirectToPage("./Index");
        }

    }
}
