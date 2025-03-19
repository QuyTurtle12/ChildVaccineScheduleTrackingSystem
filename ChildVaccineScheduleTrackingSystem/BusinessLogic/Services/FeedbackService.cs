using AutoMapper;
using BusinessLogic.DTOs.FeedbackDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        public FeedbackService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Get list of feedbacks
        public async Task<PaginatedList<GetFeedbackDTO>> GetFeedbacks(int index, int pageSize, string? idSearch, string? userIdSearch, string? customerNameSearch, int? ratingSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "Please enter a valid index or pageSize!");
            }

            IQueryable<Feedback> query = _unitOfWork.GetRepository<Feedback>()
                .Entities
                .Where(f => !f.DeletedTime.HasValue)
                .Include(f => f.User);

            // Search by feedback id
            if (!string.IsNullOrWhiteSpace(idSearch))
            {
                query = query.Where(f => f.Id == Guid.Parse(idSearch));
            }

            // Search by user id
            if (!string.IsNullOrWhiteSpace(userIdSearch))
            {
                query = query.Where(f => f.UserId == Guid.Parse(userIdSearch));
            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(customerNameSearch))
            {
                query = query.Where(f => f.User!.Name.Contains(customerNameSearch));
            }

            // Search by rating
            if (ratingSearch.HasValue)
            {
                query = query.Where(f => f.Rating == ratingSearch.Value);
            }

            // Sort by Created Time
            query = query.OrderByDescending(f => f.CreatedTime);

            PaginatedList<Feedback> resultQuery = await _unitOfWork.GetRepository<Feedback>().GetPagging(query, index, pageSize);

            IReadOnlyCollection<GetFeedbackDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetFeedbackDTO responseItem = _mapper.Map<GetFeedbackDTO>(item);
                responseItem.UserName = item.User?.Name;
                return responseItem;
            }).ToList();

            return new PaginatedList<GetFeedbackDTO>(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
            );
        }

        // Get feedback by id
        public async Task<GetFeedbackDTO> GetFeedbackById(string id)
        {
            IQueryable<Feedback> query = _unitOfWork.GetRepository<Feedback>()
                .Entities
                .Include(f => f.User);

            Feedback? feedback = await query
                .Where(f => f.Id == Guid.Parse(id))
                .FirstOrDefaultAsync();

            if (feedback == null || feedback.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "No response found!");
            }

            GetFeedbackDTO responseItem = _mapper.Map<GetFeedbackDTO>(feedback);
            responseItem.UserName = feedback.User?.Name;

            return responseItem;
        }

        // Create new feedback
        public async Task CreateFeedback(PostFeedbackDTO postFeedback)
        {
            // Validate Rating
            if (postFeedback.Rating < 1 || postFeedback.Rating > 5)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "The rating score must be between 1 and 5!");
            }

            // Validate UserId
            if (string.IsNullOrWhiteSpace(postFeedback.UserId))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "UserId must not be left blank!");
            }

            // Check if user exists
            var user = await _unitOfWork.GetRepository<User>()
                .Entities
                .Where(u => u.Id == Guid.Parse(postFeedback.UserId) && !u.DeletedTime.HasValue)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "User not found!");
            }

            // Map DTO to entity
            Feedback newFeedback = _mapper.Map<Feedback>(postFeedback);

            // Set audit fields (if not handled by BaseEntity)
            newFeedback.Status = 1; // Active by default

            await _unitOfWork.GetRepository<Feedback>().InsertAsync(newFeedback);
            await _unitOfWork.SaveAsync();
        }

        // Update feedback
        public async Task UpdateFeedback(PutFeedbackDTO updatedFeedback)
        {
            IQueryable<Feedback> query = _unitOfWork.GetRepository<Feedback>().Entities;

            Feedback? feedback = await query
                .Where(f => f.Id == Guid.Parse(updatedFeedback.Id))
                .FirstOrDefaultAsync();

            if (feedback == null || feedback.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "No response found!");
            }

            // Validate Rating
            if (updatedFeedback.Rating < 1 || updatedFeedback.Rating > 5)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, "BAD_REQUEST", "The rating must be from 1 to 5!");
            }

            // Map DTO to entity
            _mapper.Map(updatedFeedback, feedback);

            // Update audit fields
            feedback.LastUpdatedTime = DateTimeOffset.Now;
            // feedback.LastUpdatedBy = "Người cập nhật"; // Thêm nếu cần

            await _unitOfWork.GetRepository<Feedback>().UpdateAsync(feedback);
            await _unitOfWork.SaveAsync();
        }

        // Delete feedback
        public async Task DeleteFeedbackById(string id)
        {
            IQueryable<Feedback> query = _unitOfWork.GetRepository<Feedback>().Entities;

            Feedback? feedback = await query
                .Where(f => f.Id == Guid.Parse(id))
                .FirstOrDefaultAsync();

            if (feedback == null || feedback.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, "NOT_FOUND", "No response found!");
            }

            // Soft delete
            feedback.Status = 0;
            feedback.LastUpdatedTime = DateTimeOffset.Now;
            feedback.DeletedTime = feedback.LastUpdatedTime;
            // feedback.DeletedBy = "Người xóa";

            await _unitOfWork.GetRepository<Feedback>().UpdateAsync(feedback);
            await _unitOfWork.SaveAsync();
        }
    }
}