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

        public IndexModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
        }

        public PaginatedList<GetFeedbackDTO> FeedbackList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? idSearch = null, string? userIdSearch = null, int? ratingSearch = null)
        {
            FeedbackList = await _feedbackService.GetFeedbacks(index, pageSize, idSearch, userIdSearch, ratingSearch);
            return Page();
        }
    }
}