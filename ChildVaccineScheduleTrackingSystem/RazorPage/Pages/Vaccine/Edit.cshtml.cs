using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Vaccine
{
    public class EditModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public EditModel(IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public VaccinePutDto UpdatedVaccine { get; set; } = default!;
        [BindProperty]
        public VaccineGetDto Vaccine { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(Guid id)
        {

            var vaccine =  await _vaccineService.GetByIdAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            Vaccine = vaccine;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            /*var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userName = _jwtTokenService.GetName(jwtToken!);
            UpdatedVaccine.LastUpdatedTime = DateTime.Now;
            UpdatedVaccine.LastUpdatedBy = userName;*/

            if (await _vaccineService.UpdateAsync(Vaccine.Id, UpdatedVaccine))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return NotFound();
            }

        }
    }
}
