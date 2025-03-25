using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IVaccineRecordService
    {
        Task<IEnumerable<GetVaccineRecordDto>> GetAllAsync();
        Task<GetVaccineRecordDto?> GetByIdAsync(Guid id);
        Task<GetVaccineRecordDto> CreateAsync(PostVaccineRecordDto dto);
        Task<GetVaccineRecordDto?> UpdateAsync(Guid id, PutVaccineRecordDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<GetVaccineRecordDto>> GetByUserId(Guid userId);
    }
}
