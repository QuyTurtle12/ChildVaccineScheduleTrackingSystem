﻿using BusinessLogic.DTOs.UserDTO;
using Data.Enum;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<GetUserDTO>> GetUserAccounts(int index, int pageSize, string? idSearch, string? nameSearch, string? emailSearch, EnumRole? role);

        Task<GetUserDTO> GetUserAccount();

        Task CreateUserAccount(PostUserDTO postUserAccount);

        Task UpdateUserAccount(PutUserDTO updatedUserAccount);

        Task DeleteUserAccountById(string id);
    }
}
