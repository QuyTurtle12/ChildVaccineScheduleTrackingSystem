using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.FeedbackDTO;

namespace RazorPage.Pages.Feedback
{
    public class DetailsModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string CUSTOMER_ROLE = "Customer";

        public DetailsModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
        }

        public GetFeedbackDTO FeedbackDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            // Role Authorization
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

            GetFeedbackDTO feedback = await _feedbackService.GetFeedbackById(id);
            if (feedback == null)
            {
                return NotFound();
            }
            else
            {
                FeedbackDTO = feedback;
            }
            return Page();
        }
    }
}