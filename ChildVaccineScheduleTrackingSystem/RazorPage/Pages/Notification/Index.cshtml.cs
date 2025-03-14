using BusinessLogic.DTOs.NotificationDTO;
using BusinessLogic.Interfaces;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Notification
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public IndexModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public PaginatedList<GetNotificationDTO> Notifications { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UserIdSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MessageSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Index { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            Notifications = await _notificationService.GetNotifications(Index, PageSize, UserIdSearch, MessageSearch);
        }
    }
}