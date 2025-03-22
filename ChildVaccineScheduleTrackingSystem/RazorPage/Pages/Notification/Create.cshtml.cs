using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Notification
{
    public class CreateModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public CreateModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [BindProperty]
        public PostNotificationDTO PostNotification { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _notificationService.CreateNotification(PostNotification);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}