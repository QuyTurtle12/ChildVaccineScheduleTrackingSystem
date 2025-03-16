using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;

namespace BusinessLogic.Services
{
    public class VaccineRecordService : IVaccineRecordService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;

        public VaccineRecordService(IUOW unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetVaccineRecordDto>> GetAllAsync()
        {
            var records = await _unitOfWork.GetRepository<VaccineRecord>().GetAllAsync();
            return _mapper.Map<IEnumerable<GetVaccineRecordDto>>(records);
        }

        public async Task<GetVaccineRecordDto?> GetByIdAsync(Guid id)
        {
            var record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);
            return record != null ? _mapper.Map<GetVaccineRecordDto>(record) : null;
        }

        public async Task<GetVaccineRecordDto> CreateAsync(PostVaccineRecordDto dto)
        {
            var record = _mapper.Map<VaccineRecord>(dto);
            await _unitOfWork.GetRepository<VaccineRecord>().InsertAsync(record);
            await _unitOfWork.GetRepository<VaccineRecord>().SaveAsync();
            return _mapper.Map<GetVaccineRecordDto>(record);
        }

        public async Task<GetVaccineRecordDto?> UpdateAsync(Guid id, PutVaccineRecordDto dto)
        {
            var record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);
            if (record == null) return null;

            _mapper.Map(dto, record);
            await _unitOfWork.GetRepository<VaccineRecord>().UpdateAsync(record);
            return _mapper.Map<GetVaccineRecordDto>(record);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);
            if (record == null) return false;

            await _unitOfWork.GetRepository<VaccineRecord>().DeleteAsync(record);
            await _unitOfWork.GetRepository<VaccineRecord>().SaveAsync();
            return true;
        }
    }
}
