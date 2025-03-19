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

        public DetailsModel(IPackageService packageService, IVaccineService vaccineService)
        {
            _packageService = packageService;
            _vaccineService = vaccineService;
        }

        public PackageGetDTO Package { get; set; } = default!;
        public IEnumerable<VaccineGetDto> Vaccines { get; set; } = default!;

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
            return Page();
        }
    }
}
