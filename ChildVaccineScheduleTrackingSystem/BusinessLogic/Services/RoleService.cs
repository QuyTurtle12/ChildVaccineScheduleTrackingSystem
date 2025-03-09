using AutoMapper;
using BusinessLogic.DTOs.RoleDTO;
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
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;

        public RoleService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetRoleDTO> GetRoleById(string? id)
        {
            IQueryable<Role> query = _unitOfWork.GetRepository<Role>().Entities;

            Role? role = await query
                .Where(r => r.Id.Equals(Guid.Parse(id)))
                .FirstOrDefaultAsync();

            // Validate role
            if (role == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "Role not found!");
            }

            // Mapping role entities to dto
            GetRoleDTO responseItem = _mapper.Map<GetRoleDTO>(role);

            return responseItem;
        }

        public async Task<PaginatedList<GetRoleDTO>> GetRoles(int index, int pageSize, string? idSearch, string? nameSearch)
        {
            // Validate index & page size
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<Role> query = _unitOfWork.GetRepository<Role>().Entities;

            // Search by role id
            if (!string.IsNullOrWhiteSpace(idSearch))
            {
                query = query.Where(r => r.Id == Guid.Parse(idSearch));
            }

            // Sort by Id
            query = query.OrderBy(r => r.Id);

            PaginatedList<Role> resultQuery = await _unitOfWork.GetRepository<Role>().GetPagging(query, index, pageSize);

            // Map role to role dto
            IReadOnlyCollection<GetRoleDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetRoleDTO responseItem = _mapper.Map<GetRoleDTO>(item);

                return responseItem;
            }).ToList();

            PaginatedList<GetRoleDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
            );

            return paginatedList;
        }
    }
}
