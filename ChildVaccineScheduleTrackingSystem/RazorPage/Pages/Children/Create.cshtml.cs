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

        public CreateModel(IChildrenService childrenService, IJwtTokenService jwtTokenService)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PostChildrenDTO NewChild { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _childrenService.CreateChildrenAccount(NewChild);

            return RedirectToPage("./Index");
        }
    }
}
