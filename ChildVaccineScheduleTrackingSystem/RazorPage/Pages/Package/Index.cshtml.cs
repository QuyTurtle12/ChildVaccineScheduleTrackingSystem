using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Package
{
    public class IndexModel : PageModel
    {
        private readonly IPackageService _packageService;

        public IndexModel(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public IEnumerable<PackageGetDTO> Package { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Package = await _packageService.GetAllAsync();
        }
    }
}
