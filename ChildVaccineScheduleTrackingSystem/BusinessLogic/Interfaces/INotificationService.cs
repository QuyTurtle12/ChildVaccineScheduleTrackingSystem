using BusinessLogic.DTOs.NotificationDTO;
using Data.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        Task<PaginatedList<GetNotificationDTO>> GetNotifications(int index, int pageSize, string? userIdSearch, string? messageSearch);
        Task<GetNotificationDTO> GetNotificationById(string id);
        Task CreateNotification(PostNotificationDTO postNotification);
        Task UpdateNotification(PutNotificationDTO updatedNotification);
        Task DeleteNotificationById(string id);
    }
}
