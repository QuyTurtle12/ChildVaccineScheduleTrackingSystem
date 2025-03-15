using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Vaccine
{
    public class IndexModel : PageModel
    {
        private readonly IVaccineService _vaccineService;

        public IndexModel(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        public IEnumerable<VaccineGetDto> Vaccine { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Vaccine = await _vaccineService.GetAllAsync();
        }
    }
}
