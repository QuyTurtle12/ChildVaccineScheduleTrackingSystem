using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;
using System.Security;

namespace RazorPage.Pages.VaccineRecord
{
    public class EditModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IJwtTokenService _jwtTokenService;

        public EditModel(IVaccineRecordService vaccineRecordService, IJwtTokenService jwtTokenService)
        {
            _vaccineRecordService = vaccineRecordService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public GetVaccineRecordDto VaccineRecordGet { get; set; } = default!;
        [BindProperty]
        public PutVaccineRecordDto VaccineRecord { get; set; } = default!;
        public string UserRole {  get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff" && loggedInUserRole.ToLower() != "customer")
            {
                return Forbid();
            }

            UserRole = loggedInUserRole.ToLower();
            var vaccine = await _vaccineRecordService.GetByIdAsync(id);
            if(vaccine == null)
            {
                return NotFound();
            }

            VaccineRecordGet = vaccine;
            VaccineRecord = new();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _vaccineRecordService.UpdateAsync(VaccineRecordGet.Id, VaccineRecord);
            if(result == null)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
