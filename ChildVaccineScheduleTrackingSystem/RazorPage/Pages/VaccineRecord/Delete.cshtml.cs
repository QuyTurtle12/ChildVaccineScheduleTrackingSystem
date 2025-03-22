using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.Services;

namespace RazorPage.Pages.VaccineRecord
{
    public class DeleteModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IJwtTokenService _jwtTokenService;

        public DeleteModel(IVaccineRecordService vaccineRecordService, IJwtTokenService jwtTokenService)
        {
            _vaccineRecordService = vaccineRecordService;
            _jwtTokenService = jwtTokenService;
        }


        [BindProperty]
        public GetVaccineRecordDto VaccineRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff")
            {
                return Forbid();
            }

            var vaccinerecord = await _vaccineRecordService.GetByIdAsync(id);

            if (vaccinerecord == null)
            {
                return NotFound();
            }
            else
            {
                VaccineRecord = vaccinerecord;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            
            var result = await _vaccineRecordService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest("Invalid id");
            }

            return RedirectToPage("./Index");
        }
    }
}
