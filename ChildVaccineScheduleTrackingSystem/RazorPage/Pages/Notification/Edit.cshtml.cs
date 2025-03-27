using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Notification
{
    public class EditModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public EditModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [BindProperty]
        public PutNotificationDTO PutNotification { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var notification = await _notificationService.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }

            PutNotification = new PutNotificationDTO
            {
                Id = notification.Id,
                Message = notification.Message,
                Status = notification.Status
            };

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
                await _notificationService.UpdateNotification(PutNotification);
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