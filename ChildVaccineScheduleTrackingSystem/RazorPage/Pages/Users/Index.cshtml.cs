using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using BusinessLogic.DTOs.UserDTO;
using Data.Enum;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string ADMIN = "Admin";

        public IndexModel(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        public PaginatedList<GetUserDTO> UserDTOList { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? idSearch = null, string? nameSearch = null, string? emailSearch = null, EnumRole? role = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            var userRole = _jwtTokenService.GetRole(jwtToken!);

            //// Role Authorization
            //if (userRole == null || userRole != ADMIN)
            //{
            //    return Forbid();
            //}

            UserDTOList = await _userService.GetUserAccounts(index, pageSize, idSearch, nameSearch, emailSearch, role);

            return Page();
        }
    }
}
