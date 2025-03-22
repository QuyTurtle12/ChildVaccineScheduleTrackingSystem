using BusinessLogic.DTOs.ChildrenDTO;
using Data.PaggingItem;

namespace BusinessLogic.Interfaces
{
    public interface IChildrenService
    {
        Task<PaginatedList<GetChildrenDTO>> GetChildrenList(int index, int pageSize, string? idSearch, string? nameSearch, string? parentEmailSearch);

        Task<GetChildrenDTO> GetChildrenAccount(string id);

        Task CreateChildrenAccount(PostChildrenDTO postChildrenAccount);

        Task UpdateChildrenAccount(PutChildrenDTO updatedpostChildrenAccountAccount);

        Task DeleteChildrenAccountById(string id);
        Task<IEnumerable<GetChildrenDTO>> GetChildrenListByUserPhoneNumber(string phoneNumber);
    }
}
