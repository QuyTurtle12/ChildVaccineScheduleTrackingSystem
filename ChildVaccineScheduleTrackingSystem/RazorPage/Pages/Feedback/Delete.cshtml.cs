using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.FeedbackDTO;

namespace RazorPage.Pages.Feedback
{
    public class DeleteModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtTokenService _jwtTokenService;

        public DeleteModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public GetFeedbackDTO FeedbackDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
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

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _feedbackService.DeleteFeedbackById(id);
                TempData["SuccessMessage"] = "Feedback deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                throw;
            }
        }
    }
}