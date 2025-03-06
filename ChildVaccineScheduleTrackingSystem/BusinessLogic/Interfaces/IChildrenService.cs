using BusinessLogic.DTOs.ChildrenDTO;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface IChildrenService
    {
        Task<PaginatedList<GetChildrenDTO>> GetChildrenList(int index, int pageSize, string? idSearch, string? nameSearch, string? parentEmailSearch);

        Task<GetChildrenDTO> GetChildrenAccount(string Id);

        Task CreateChildrenAccount(PostChildrenDTO postChildrenAccount);

        Task UpdateChildrenAccount(PutChildrenDTO updatedpostChildrenAccountAccount);

        Task DeleteChildrenAccountById(string id);
    }
}
