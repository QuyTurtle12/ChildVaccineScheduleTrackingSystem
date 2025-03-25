using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.ExceptionCustom;
using Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class VaccineRecordService : IVaccineRecordService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IChildrenService _childrenService;
        public VaccineRecordService(IUOW unitOfWork, IMapper mapper, IChildrenService childrenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _childrenService = childrenService;
        }

        public async Task<IEnumerable<GetVaccineRecordDto>> GetAllAsync()
        {
            IEnumerable<VaccineRecord> records = await _unitOfWork.GetRepository<VaccineRecord>()
                .Entities
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync();

            IEnumerable<GetVaccineRecordDto> result = _mapper.Map<IEnumerable<GetVaccineRecordDto>>(records);
            foreach(var item in result)
            {
                await AssignVaccineAndChildNameToGetDto(item);
            }
            return result;
        }

        public async Task<GetVaccineRecordDto?> GetByIdAsync(Guid id)
        {
            VaccineRecord? record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);

            if(record == null)
            {
                return null;
            }
            GetVaccineRecordDto result = _mapper.Map<GetVaccineRecordDto>(record);
            await AssignVaccineAndChildNameToGetDto(result);
            return result;
        }

        public async Task<GetVaccineRecordDto> CreateAsync(PostVaccineRecordDto dto)
        {
            if(dto.ChildId == Guid.Empty || dto.VaccineId == Guid.Empty)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid child id or vaccine id");
            }

            if (!await _unitOfWork.GetRepository<Child>()
                .Entities
                .AnyAsync(c => c.Id == dto.ChildId)) 
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Child does not exist");
            if (!await _unitOfWork.GetRepository<Vaccine>()
                .Entities
                .AnyAsync(c => c.Id == dto.VaccineId))
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Vaccine does not exist");

            var record = _mapper.Map<VaccineRecord>(dto);
            await _unitOfWork.GetRepository<VaccineRecord>().InsertAsync(record);
            await _unitOfWork.GetRepository<VaccineRecord>().SaveAsync();

            GetVaccineRecordDto result = _mapper.Map<GetVaccineRecordDto>(record);
            await AssignVaccineAndChildNameToGetDto(result);
            return result;
        }

        public async Task<GetVaccineRecordDto?> UpdateAsync(Guid id, PutVaccineRecordDto dto)
        {
            var record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);
            if (record == null) return null;

            _mapper.Map(dto, record);
            await _unitOfWork.GetRepository<VaccineRecord>().UpdateAsync(record);
            await _unitOfWork.GetRepository<VaccineRecord>().SaveAsync();

            GetVaccineRecordDto result = _mapper.Map<GetVaccineRecordDto>(record);
            await AssignVaccineAndChildNameToGetDto(result);
            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var record = await _unitOfWork.GetRepository<VaccineRecord>().GetByIdAsync(id);
            if (record == null) return false;

            await _unitOfWork.GetRepository<VaccineRecord>().DeleteAsync(record);
            await _unitOfWork.GetRepository<VaccineRecord>().SaveAsync();
            return true;
        }
        private async Task AssignVaccineAndChildNameToGetDto(GetVaccineRecordDto dto)
        {
            dto.ChildName = await _unitOfWork.GetRepository<Child>()
                .Entities
                .Where(c => c.Id == dto.ChildId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
            dto.VaccineName = await _unitOfWork.GetRepository<Vaccine>()
                .Entities
                .Where(v => v.Id == dto.VaccineId)
                .Select(v => v.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetVaccineRecordDto>> GetByUserId(Guid userId)
        {
            IEnumerable<Guid> childrenIds = await _unitOfWork.GetRepository<Child>()
                .Entities
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .ToListAsync();
            List<VaccineRecord> records = new List<VaccineRecord>();
            foreach (Guid childId in childrenIds) 
            {
                records.AddRange(await _unitOfWork.GetRepository<VaccineRecord>()
                    .Entities
                    .Where(vr => vr.ChildId == childId)
                    .ToListAsync());
            }
            records.OrderByDescending(vr => vr.ChildId);
            IEnumerable<GetVaccineRecordDto> result = _mapper.Map<IEnumerable<GetVaccineRecordDto>>(records);
            foreach (var item in result)
            {
                await AssignVaccineAndChildNameToGetDto(item);
            }

            return result;
        }
    }
}
