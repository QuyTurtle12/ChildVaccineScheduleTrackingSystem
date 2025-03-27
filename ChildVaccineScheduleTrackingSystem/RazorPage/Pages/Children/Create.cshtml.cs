using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.ChildrenDTO;

namespace RazorPage.Pages.Children
{
    public class CreateModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string CUSTOMER_ROLE = "Customer";
        public CreateModel(IChildrenService childrenService, IJwtTokenService jwtTokenService)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            return Page();
        }

        [BindProperty]
        public PostChildrenDTO NewChild { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _childrenService.CreateChildrenAccount(NewChild);

            return RedirectToPage("./Index");
        }
    }
}
