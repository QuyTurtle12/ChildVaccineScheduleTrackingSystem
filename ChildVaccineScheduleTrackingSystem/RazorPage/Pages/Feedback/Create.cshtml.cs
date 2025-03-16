using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.FeedbackDTO;

namespace RazorPage.Pages.Feedback
{
    public class CreateModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;

        public CreateModel(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [BindProperty]
        public PostFeedbackDTO Feedback { get; set; } = new PostFeedbackDTO();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _feedbackService.CreateFeedback(Feedback);
                return RedirectToPage("/Feedback/Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the feedback.");
                return Page();
            }
        }
    }
}