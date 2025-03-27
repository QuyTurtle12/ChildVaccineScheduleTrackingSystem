using BusinessLogic.DTOs.RoleDTO;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface IRoleService
    {
        Task<PaginatedList<GetRoleDTO>> GetRoles(int index, int pageSize, string? idSearch, string? nameSearch);
        Task<GetRoleDTO> GetRoleById(string? id);
    }
}
