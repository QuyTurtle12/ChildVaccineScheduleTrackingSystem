using BusinessLogic.DTOs.FeedbackDTO;
using Data.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IFeedbackService
    {
        Task<PaginatedList<GetFeedbackDTO>> GetFeedbacks(int index, int pageSize, string? idSearch, string? userIdSearch, int? ratingSearch);
        Task<GetFeedbackDTO> GetFeedbackById(string id);
        Task CreateFeedback(PostFeedbackDTO postFeedback);
        Task UpdateFeedback(PutFeedbackDTO updatedFeedback);
        Task DeleteFeedbackById(string id);
    }
}
