using BusinessLogic.DTOs.SystemAccountDTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public RegisterDTO RegisterDTO { get; set; }

        public string Message { get; set; }

        public RegisterModel(ISystemAccountService systemAccountService, ILogger<RegisterModel> logger)
        {
            _systemAccountService = systemAccountService;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _systemAccountService.Register(RegisterDTO);
            if (result)
            {
                Message = "Registration successful. You can now login.";
                return RedirectToPage("/Login");
            }
            else
            {
                Message = "Registration failed. Email might already be in use.";
                return Page();
            }
        }
    }
}
