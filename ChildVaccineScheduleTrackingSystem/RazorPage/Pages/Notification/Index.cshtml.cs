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
        private readonly IJwtTokenService _jwtTokenService;

        private const string STAFF_ROLE = "Staff";
        private const string CUSTOMER_ROLE = "Customer";

        public IndexModel(INotificationService notificationService, IJwtTokenService jwtTokenService)
        {
            _notificationService = notificationService;
            _jwtTokenService = jwtTokenService;
        }

        public PaginatedList<GetNotificationDTO> Notifications { get; set; } = default!;
        public string LoggedInUserRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 10, string? userIdSearch = null, string? messageSearch = null, string? messageCreatorId = null)
        {
            // Role Authorization
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            LoggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (LoggedInUserRole == null) return Unauthorized();

            if (LoggedInUserRole != STAFF_ROLE && LoggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            if (LoggedInUserRole == CUSTOMER_ROLE) // Get notifications of current logged in user
            {
                userIdSearch = _jwtTokenService.GetId(jwtToken!);
                Notifications = await _notificationService.GetNotifications(index, pageSize, userIdSearch, messageSearch, messageCreatorId);
            }
            else if (LoggedInUserRole == STAFF_ROLE) // Get notifications that the logged in staff has created
            {
                messageCreatorId = _jwtTokenService.GetId(jwtToken!);
                Notifications = await _notificationService.GetNotifications(index, pageSize, userIdSearch, messageSearch, messageCreatorId);
            }

            return Page();
        }
    }
}