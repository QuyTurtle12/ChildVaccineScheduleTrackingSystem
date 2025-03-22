using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.ChildrenDTO;
using AutoMapper;

namespace RazorPage.Pages.Children
{
    public class EditModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        private const string CUSTOMER_ROLE = "Customer";

        public EditModel(IChildrenService childrenService, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        [BindProperty]
        public PutChildrenDTO EditChild { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            GetChildrenDTO ChildDTO = await _childrenService.GetChildrenAccount(id);
            if (ChildDTO == null)
            {
                return NotFound();
            }
            EditChild = _mapper.Map<PutChildrenDTO>(ChildDTO);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE) // Customer's function
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _childrenService.UpdateChildrenAccount(EditChild);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }

            return RedirectToPage("./Index");
        }
    }
}
