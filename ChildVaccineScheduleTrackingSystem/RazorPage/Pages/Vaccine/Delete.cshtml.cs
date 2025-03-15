using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.Vaccine
{
    public class DeleteModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public DeleteModel(IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            /*var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userName = _jwtTokenService.GetName(jwtToken!);
            Vaccine.DeletedTime = DateTime.Now;
            Vaccine.DeletedBy = userName;*/

            if (await _vaccineService.SoftDeleteAsync(id))
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
