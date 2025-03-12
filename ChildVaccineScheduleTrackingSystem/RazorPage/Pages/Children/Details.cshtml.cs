using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.ChildrenDTO;

namespace RazorPage.Pages.Children
{
    public class DetailsModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IJwtTokenService _jwtTokenService;

        public DetailsModel(IChildrenService childrenService, IJwtTokenService jwtTokenService)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
        }

        public GetChildrenDTO ChildDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetChildrenDTO child = await _childrenService.GetChildrenAccount(id);
            if (child == null)
            {
                return NotFound();
            }
            else
            {
                ChildDTO = child;
            }
            return Page();
        }
    }
}
