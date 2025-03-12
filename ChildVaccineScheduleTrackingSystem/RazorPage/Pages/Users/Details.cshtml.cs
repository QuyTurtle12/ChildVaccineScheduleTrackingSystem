using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.DTOs.UserDTO;
using BusinessLogic.Interfaces;

namespace RazorPage.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public DetailsModel(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        public GetUserDTO UserDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetUserDTO user = await _userService.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                UserDTO = user;
            }
            return Page();
        }
    }
}
