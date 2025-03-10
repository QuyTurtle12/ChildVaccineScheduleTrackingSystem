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

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public PaginatedList<GetUserDTO> User { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? idSearch = null, string? nameSearch = null, string? emailSearch = null, EnumRole? role = null)
        {
            User = await _userService.GetUserAccounts(index, pageSize, idSearch, nameSearch, emailSearch, role);

            return Page();
        }
    }
}
