using BusinessLogic.DTOs.NotificationDTO;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        Task<PaginatedList<GetNotificationDTO>> GetNotifications(int index, int pageSize, string? userIdSearch, string? messageSearch, string? messageCreatorId);
        Task<GetNotificationDTO> GetNotificationById(string id);
        Task CreateNotification(PostNotificationDTO postNotification);
        Task UpdateNotification(PutNotificationDTO updatedNotification);
        Task DeleteNotificationById(string id);
    }
}
