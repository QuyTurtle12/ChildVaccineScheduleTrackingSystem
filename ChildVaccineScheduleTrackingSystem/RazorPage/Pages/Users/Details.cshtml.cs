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
        public string loggedInUserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            // Role Authorization
            if (loggedInUserRole == null)
            {
                return Forbid();
            }

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
