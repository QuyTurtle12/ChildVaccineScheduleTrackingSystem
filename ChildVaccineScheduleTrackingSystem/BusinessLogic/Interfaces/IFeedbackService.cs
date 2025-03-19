using BusinessLogic.DTOs.FeedbackDTO;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface IFeedbackService
    {
        Task<PaginatedList<GetFeedbackDTO>> GetFeedbacks(int index, int pageSize, string? idSearch, string? userIdSearch, string? customerNameSearch, int? ratingSearch);
        Task<GetFeedbackDTO> GetFeedbackById(string id);
        Task CreateFeedback(PostFeedbackDTO postFeedback);
        Task UpdateFeedback(PutFeedbackDTO updatedFeedback);
        Task DeleteFeedbackById(string id);
    }
}
