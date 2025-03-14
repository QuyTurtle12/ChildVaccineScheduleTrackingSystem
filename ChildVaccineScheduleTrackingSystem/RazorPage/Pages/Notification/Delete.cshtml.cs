using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Notification
{
    public class DeleteModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public DeleteModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public GetNotificationDTO Notification { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Notification = await _notificationService.GetNotificationById(id);
            if (Notification == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                await _notificationService.DeleteNotificationById(id);
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