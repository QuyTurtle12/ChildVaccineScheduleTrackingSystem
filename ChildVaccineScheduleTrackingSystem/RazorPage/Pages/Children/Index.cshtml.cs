using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using BusinessLogic.DTOs.ChildrenDTO;
using BusinessLogic.DTOs.UserDTO;

namespace RazorPage.Pages.Children
{
    public class IndexModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string CUSTOMER_ROLE = "Customer";
        public IndexModel(IChildrenService childrenService, IJwtTokenService jwtTokenService, ITokenService tokenService, IUserService userService)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
            _tokenService = tokenService;
            _userService = userService;
        }

        public PaginatedList<GetChildrenDTO> ChildrenList { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? idSearch = null, string? nameSearch = null, string? parentEmailSearch = null)
        {
            ChildrenList = await _childrenService.GetChildrenList(index, pageSize, idSearch, nameSearch, parentEmailSearch);

            //string? currentLoggedInUserId = _tokenService.GetCurrentUserId();
            //GetUserDTO? currentUser = await _userService.GetUserProfile(currentLoggedInUserId);
            //if (currentUser != null && currentUser.RoleName == CUSTOMER_ROLE) 
            //{
            //    // Get a list of children of logged in user
            //    parentEmailSearch = currentUser.Email;
            //    ChildrenList = await _childrenService.GetChildrenList(index, pageSize, idSearch, nameSearch, parentEmailSearch);
            //}

            return Page();
        }
    }
}
