using BusinessLogic.DTOs;
namespace BusinessLogic.Interfaces
{
    public interface IPackageService
    {
        Task<IEnumerable<PackageGetDTO>> GetAllAsync();
        Task<PackageGetDTO> GetByIdAsync(Guid id);
        Task<PackageGetDTO> CreateAsync(PackagePostDTO dto);
        Task<bool> UpdateAsync(Guid id, PackagePutDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
