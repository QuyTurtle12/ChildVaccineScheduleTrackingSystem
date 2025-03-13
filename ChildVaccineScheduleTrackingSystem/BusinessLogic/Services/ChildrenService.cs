using AutoMapper;
using BusinessLogic.DTOs.ChildrenDTO;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ChildrenService : IChildrenService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly ITokenService _tokenService;

        public ChildrenService(IMapper mapper, IUOW unitOfWork, ITokenService tokenService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task CreateChildrenAccount(PostChildrenDTO postChildrenAccount)
        {
            // Validate Children Name
            if (string.IsNullOrWhiteSpace(postChildrenAccount.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Children Name");
            }

            // Mapping user dto to entities
            Child newChild = _mapper.Map<Child>(postChildrenAccount);

            // Get parent id
            string? parentId = _tokenService.GetCurrentUserId();

            // Add Audit Fields
            newChild.CreatedBy = parentId;
            newChild.LastUpdatedBy = newChild.CreatedBy;

            await _unitOfWork.GetRepository<Child>().InsertAsync(newChild);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteChildrenAccountById(string id)
        {
            IQueryable<Child> query = _unitOfWork.GetRepository<Child>().Entities;

            Child? child = await query.Where(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();

            // Validate if child exist
            if (child == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Child not found!");
            }

            // Get edited user id
            string? updatedPersonId = _tokenService.GetCurrentUserId();

            // Update audit fields
            child.Status = 0;
            child.LastUpdatedTime = DateTimeOffset.Now;
            child.DeletedTime = child.LastUpdatedTime;
            child.LastUpdatedBy = updatedPersonId;

            await _unitOfWork.GetRepository<Child>().UpdateAsync(child);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateChildrenAccount(PutChildrenDTO updatedChildrenAccount)
        {
            IQueryable<Child> query = _unitOfWork.GetRepository<Child>().Entities;

            Child? child = await query.Where(u => u.Id == Guid.Parse(updatedChildrenAccount.Id)).FirstOrDefaultAsync();

            // Validate if child exist
            if (child == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Child not found!");
            }

            _mapper.Map(updatedChildrenAccount, child);

            // Get edited user id
            string? updatedPersonId = _tokenService.GetCurrentUserId();

            // Update audit fields
            child.LastUpdatedTime = DateTimeOffset.Now;
            child.LastUpdatedBy = updatedPersonId;

            await _unitOfWork.GetRepository<Child>().UpdateAsync(child);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GetChildrenDTO> GetChildrenAccount(string id)
        {
            IQueryable<Child> query = _unitOfWork.GetRepository<Child>().Entities;

            Child? child = await query
                .Where(c => c.Id.Equals(Guid.Parse(id)))
                .Include(c => c.User)
                .FirstOrDefaultAsync();

            // Validate child
            if (child == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Child not found!");
            }

            // Mapping child entities to dto
            GetChildrenDTO responseItem = _mapper.Map<GetChildrenDTO>(child);

            responseItem.ParentEmail = child.User!.Email!;
            responseItem.ParentName = child.User.Name;

            return responseItem;
        }

        public async Task<PaginatedList<GetChildrenDTO>> GetChildrenList(int index, int pageSize, string? idSearch, string? nameSearch, string? parentEmailSearch)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable <Child> query = _unitOfWork.GetRepository<Child>().Entities.Include(c => c.User);

            // Search by user id
            if (!string.IsNullOrWhiteSpace(idSearch))
            {
                query = query.Where(u => u.Id == Guid.Parse(idSearch));

            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.Name!.Contains(nameSearch));
            }

            // Search by parent email
            if (!string.IsNullOrWhiteSpace(parentEmailSearch))
            {
                // query = query.Where(u => u.AccountEmail!.Equals(emailSearch));
                parentEmailSearch = parentEmailSearch.Trim();
                query = query.Where(c => c.User!.Email!.Trim().ToLower().Contains(parentEmailSearch.ToLower()));
            }

            // Sort by Id
            query = query.OrderBy(c => c.Age);

            PaginatedList<Child> resultQuery = await _unitOfWork.GetRepository<Child>().GetPagging(query, index, pageSize);

            // Map entities to dto
            IReadOnlyCollection<GetChildrenDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetChildrenDTO responseItem = _mapper.Map<GetChildrenDTO>(item);

                responseItem.ParentName = item.User!.Name;
                responseItem.ParentEmail = item.User.Email!;

                return responseItem;
            }).ToList();

            PaginatedList<GetChildrenDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }
    }
}
