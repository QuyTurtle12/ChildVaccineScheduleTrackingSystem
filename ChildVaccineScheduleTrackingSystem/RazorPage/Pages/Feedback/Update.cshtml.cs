using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.FeedbackDTO;
using AutoMapper;

namespace RazorPage.Pages.Feedback
{
    public class UpdateModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        private const string STAFF_ROLE = "Staff";

        public UpdateModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        [BindProperty]
        public PutFeedbackDTO UpdateFeedback { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            // Role Authorization
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != STAFF_ROLE)
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }

            GetFeedbackDTO feedbackDTO = await _feedbackService.GetFeedbackById(id);
            if (feedbackDTO == null)
            {
                return NotFound();
            }
            UpdateFeedback = _mapper.Map<PutFeedbackDTO>(feedbackDTO);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Role Authorization
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != STAFF_ROLE)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _feedbackService.UpdateFeedback(UpdateFeedback);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }

            return RedirectToPage("./Index");
        }
    }
}