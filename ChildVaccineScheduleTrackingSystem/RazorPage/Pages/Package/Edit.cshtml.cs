using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Package
{
    public class EditModel : PageModel
    {
        private readonly IPackageService _packageService;
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public EditModel(IPackageService packageService, IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _packageService = packageService;
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        public IEnumerable<VaccineGetDto> VaccinesInPackage { get; set; } = default!;
        public IEnumerable<VaccineGetDto> AllVaccines { get; set; } = default!;
        [BindProperty]
        public PackageGetDTO PackageGetDto { get; set; } = default!;
        [BindProperty]
        public PackagePutDTO Package { get; set; } = default!;
        public List<string> Types { get; set; } = new List<string> { PackageType.Single.ToString(), PackageType.LongTerm.ToString() };
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff")
            {
                return Forbid();
            }

            var package =  await _packageService.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            else
            {
                PackageGetDto = package;
                VaccinesInPackage = await _vaccineService.GetVaccineByPackageId(id);
                AllVaccines = await _vaccineService.GetAllAsync();
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
            await _packageService.UpdatePackageVaccines(PackageGetDto.Id, Package.SelectedVaccineIds);

            return RedirectToPage("./Index");
        }

    }
}
