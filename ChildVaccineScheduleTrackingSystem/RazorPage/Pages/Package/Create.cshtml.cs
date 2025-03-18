using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using Data.Enum;

namespace RazorPage.Pages.Package
{
    public class CreateModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IVaccineService _vaccineService;

        public CreateModel(IPackageService packageService, IVaccineService vaccineService)
        {
            _packageService = packageService;
            _vaccineService = vaccineService;
        }

        

        public List<VaccineGetDto> AllVaccines { get; set; } = default!;
        [BindProperty]
        public PackagePostDTO Package { get; set; } = default!;
        public List<string> Types { get; set; } = new List<string> { PackageType.Single.ToString(), PackageType.LongTerm.ToString() };

        public async Task<IActionResult> OnGet()
        {
            AllVaccines = (List<VaccineGetDto>) await _vaccineService.GetAllAsync();
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PackageGetDTO package = await _packageService.CreateAsync(Package);
            await _packageService.UpdatePackageVaccines(package.Id, Package.SelectedVaccineIds);

            return RedirectToPage("./Index");
        }
    }
}
