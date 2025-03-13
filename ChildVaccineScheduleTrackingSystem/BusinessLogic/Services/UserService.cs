using AutoMapper;
using BusinessLogic.DTOs.UserDTO;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.Enum;
using Data.ExceptionCustom;
using Data.Interface;
using Data.PaggingItem;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly ITokenService _tokenService;

        private const int GUEST = 1;
        private const int CUSTOMER = 2;
        private const int STAFF = 3;
        private const int ADMIN = 4;
        private const int STARTING_NUMBER = 0;

        public UserService(IMapper mapper, IUOW unitOfWork, ITokenService tokenService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        // Get list of system user
        public async Task<PaginatedList<GetUserDTO>> GetUserAccounts(int index, int pageSize, string? idSearch, string? nameSearch, string? emailSearch, EnumRole? role)
        {
            // Validate index & page size
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<User> query = _unitOfWork.GetRepository<User>()
                                                .Entities
                                                .Where(u => !u.DeletedTime.HasValue)
                                                .Include(u => u.Role);

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

            // Search by email
            if (!string.IsNullOrWhiteSpace(emailSearch))
            {
                // query = query.Where(u => u.AccountEmail!.Equals(emailSearch));
                emailSearch = emailSearch.Trim();
                query = query.Where(u => u.Email!.Trim().ToLower().Contains(emailSearch.ToLower()));
            }

            // Search by role
            switch (role)
            {
                case (EnumRole?)STAFF:
                    query = query.Where(u => u.Role!.Name == EnumRole.Staff.ToString());
                    break;
                case (EnumRole?)CUSTOMER:
                    query = query.Where(u => u.Role!.Name == EnumRole.Customer.ToString());
                    break;
                default:
                    break;
            }

            // Exclude Admin
            query = query.Where(u => !u.Role!.Name.Equals("Admin"));

            // Sort by Id
            query = query.OrderBy(u => u.Id);

            PaginatedList<User> resultQuery = await _unitOfWork.GetRepository<User>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetUserDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetUserDTO responseItem = _mapper.Map<GetUserDTO>(item);

                responseItem.RoleName = item.Role!.Name == EnumRole.Staff.ToString() ? "Staff" : "Customer";

                return responseItem;
            }).ToList();

            PaginatedList<GetUserDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }

        // Get 1 user account by id
        public async Task<GetUserDTO> GetUserProfile(string id)
        {

            IQueryable<User> query = _unitOfWork.GetRepository<User>().Entities.Include(u => u.Role);

            User? user = await query.Where(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();

            // Validate user
            if (user == null || user.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "User not found!");
            }

            // Mapping user entities to dto
            GetUserDTO responseItem = _mapper.Map<GetUserDTO>(user);

            responseItem.RoleName = user.Role!.Name == EnumRole.Staff.ToString() ? "Staff" : "Customer";

            return responseItem;
        }

        // Update User Info
        public async Task UpdateUserAccount(PutUserDTO updatedUserAccount)
        {

            IQueryable<User> query = _unitOfWork.GetRepository<User>().Entities;

            User? user = await query.Where(u => u.Id == Guid.Parse(updatedUserAccount.Id)).FirstOrDefaultAsync();

            // Validate user
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "User not found!");
            }

            _mapper.Map(updatedUserAccount, user);

            // Update audit fields
            user.LastUpdatedTime = DateTimeOffset.Now;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        // Delete User
        public async Task DeleteUserAccountById(string id)
        {
            IQueryable<User> query = _unitOfWork.GetRepository<User>().Entities;

            User? user = await query.Where(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();

            // Validate user
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found!");
            }

            // Update audit fields
            user.Status = 0;
            user.LastUpdatedTime = DateTimeOffset.Now;
            user.DeletedTime = user.LastUpdatedTime;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateUserAccount(PostUserDTO postUserAccount)
        {
            // Validate Account Name
            if (string.IsNullOrWhiteSpace(postUserAccount.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Account Name");
            }

            // Validate Account Email
            if (string.IsNullOrWhiteSpace(postUserAccount.Email) || (!postUserAccount.Email.Contains("@") || postUserAccount.Email.Count(c => c == '@') > 1))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Account Email");
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(postUserAccount.Password))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Password");
            }

            // Validate Password & Confirm Password
            if (!postUserAccount.Password.Equals(postUserAccount.ConfirmedPassword))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Password not matched");
            }

            // Mapping user dto to entities
            User newUser = _mapper.Map<User>(postUserAccount);

            // Get role Id by role name
            Guid roleId = await _unitOfWork.GetRepository<Role>().Entities
                            .Where(r => r.Name.Equals(postUserAccount.RoleName.ToString()))
                            .Select(r => r.Id)
                            .FirstOrDefaultAsync();

            newUser.RoleId = roleId;

            newUser.Password = BCrypt.Net.BCrypt.HashPassword(postUserAccount.Password);

            await _unitOfWork.GetRepository<User>().InsertAsync(newUser);
            await _unitOfWork.SaveAsync();
        }
    }
}
