using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using BusinessLogic.DTOs.FeedbackDTO;

namespace RazorPage.Pages.Feedback
{
    public class IndexModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string STAFF_ROLE = "Staff";

        public IndexModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
        }

        public PaginatedList<GetFeedbackDTO> FeedbackList { get; set; } = default!;
        public string? CustomerNameSearch { get; set; }

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? idSearch = null, string? userIdSearch = null, string? customerNameSearch = null, int? ratingSearch = null)
        {
            // Role Authorization
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != STAFF_ROLE)
            {
                return Forbid();
            }

            CustomerNameSearch = customerNameSearch;

            // Get all feedbacks
            FeedbackList = await _feedbackService.GetFeedbacks(index, pageSize, idSearch, userIdSearch, customerNameSearch, ratingSearch);
            return Page();
        }
    }
}