using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPage.Pages.VaccineRecord
{
    public class CreateModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;
        private readonly IChildrenService _childrenService;
        private readonly IVaccineService _vaccineService;
        private readonly IJwtTokenService _jwtTokenService;

        public CreateModel(IVaccineRecordService vaccineRecordService,
                           IChildrenService childrenService,
                           IVaccineService vaccineService,
                           IJwtTokenService jwtTokenService)
        {
            _vaccineRecordService = vaccineRecordService;
            _childrenService = childrenService;
            _vaccineService = vaccineService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public PostVaccineRecordDto VaccineRecord { get; set; } = default!;

        [BindProperty]
        public string PhoneNumber { get; set; } = string.Empty;

        public List<SelectListItem> ChildrenList { get; set; } = new();
        public List<SelectListItem> VaccineList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? phoneNumber)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole.ToLower() != "staff")
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                PhoneNumber = phoneNumber;
                ChildrenList = (await _childrenService.GetChildrenListByUserPhoneNumber(phoneNumber))
                              .Select(c => new SelectListItem
                              {
                                  Value = c.Id.ToString(),
                                  Text = c.Name
                              }).ToList();
            }
            else
            {
                ChildrenList = new List<SelectListItem>(); // Empty if no phone number
            }

            var vaccines = await _vaccineService.GetAllAsync();
            VaccineList = vaccines.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

            return Page(); // Ensure you return IActionResult
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(VaccineRecord.ChildId == Guid.Empty || VaccineRecord.VaccineId == Guid.Empty)
                return BadRequest("Child or vaccine is empty");

            var result = await _vaccineRecordService.CreateAsync(VaccineRecord);

            if (result == null)
            {
                return BadRequest("Create vaccine record failed");
            }

            return RedirectToPage("./Index");
        }
    }
}

