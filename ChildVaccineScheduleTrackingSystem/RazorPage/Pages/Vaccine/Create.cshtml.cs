using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Vaccine
{
    public class CreateModel : PageModel
    {
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public CreateModel(IVaccineService vaccineService, IJwtTokenService jwtTokenService)
        {
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public VaccinePostDto Vaccine { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            /*var jwtToken = HttpContext.Session.GetString("jwt_token");
            string userName = _jwtTokenService.GetName(jwtToken!);
            Vaccine.CreatedBy = userName;
            Vaccine.CreatedTime = DateTime.Now;*/

            await _vaccineService.CreateAsync(Vaccine);

            return RedirectToPage("./Index");
        }
    }
}
