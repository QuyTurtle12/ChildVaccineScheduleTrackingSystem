using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IVaccineService
    {
        Task<IEnumerable<VaccineGetDto>> GetAllAsync();
        Task<VaccineGetDto> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(VaccinePostDto dto);
        Task<bool> UpdateAsync(Guid id, VaccinePutDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
