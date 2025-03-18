using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.FeedbackDTO;

namespace RazorPage.Pages.Feedback
{
    public class CreateModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string CUSTOMER_ROLE = "Customer";

        public CreateModel(IFeedbackService feedbackService, IJwtTokenService jwtTokenService)
        {
            _feedbackService = feedbackService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public PostFeedbackDTO Feedback { get; set; } = new PostFeedbackDTO();

        public IActionResult OnGet(string id)
        {
            // Role Authorization
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);
            

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            // Pre-populate appointment id
            if (!string.IsNullOrEmpty(id))
            {
                Feedback.AppointmentId = id;
            }

            // Pre-populate user id
            string loggedInUserId = _jwtTokenService.GetId(jwtToken!);
            Feedback.UserId = loggedInUserId;

            // Pre-populate rating
            Feedback.Rating = 5;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Role Authorization
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

            try
            {
                await _feedbackService.CreateFeedback(Feedback);
                return RedirectToPage("/Appointments/Index");
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