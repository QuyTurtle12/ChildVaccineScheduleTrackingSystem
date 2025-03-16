using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Vaccine
{
    public class DetailsModel : PageModel
    {
        private readonly IVaccineService _vaccineService;

        public DetailsModel(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        public VaccineGetDto Vaccine { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vaccine = await _vaccineService.GetByIdAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            else
            {
                Vaccine = vaccine;
            }
            return Page();
        }
    }
}
