using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.UserDTO;

namespace RazorPage.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public CreateModel(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PostUserDTO NewUser { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.CreateUserAccount(NewUser);

            return RedirectToPage("./Index");
        }
    }
}
