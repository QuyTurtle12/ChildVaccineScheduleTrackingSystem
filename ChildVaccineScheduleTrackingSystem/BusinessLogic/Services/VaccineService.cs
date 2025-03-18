using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;

        public VaccineService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VaccineGetDto>> GetAllAsync()
        {
            IEnumerable<Vaccine> vaccines = await _unitOfWork.GetRepository<Vaccine>()
                .Entities
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync();
            return _mapper.Map<IEnumerable<VaccineGetDto>>(vaccines);
        }

        public async Task<VaccineGetDto> GetByIdAsync(Guid id)
        {
            Vaccine? vaccine = await _unitOfWork.GetRepository<Vaccine>().GetByIdAsync(id);
            return _mapper.Map<VaccineGetDto>(vaccine);
        }

        public async Task<bool> CreateAsync(VaccinePostDto dto)
        {
            Vaccine vaccine = _mapper.Map<Vaccine>(dto);

            try
            {
                await _unitOfWork.GetRepository<Vaccine>().InsertAsync(vaccine);
                await _unitOfWork.GetRepository<Vaccine>().SaveAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed insert vaccine to db: " + e.Message);
                return false;
            }
            
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, VaccinePutDto dto)
        {
            Vaccine? vaccine = await _unitOfWork.GetRepository<Vaccine>().GetByIdAsync(id);
            if (vaccine == null) return false;
            
            _mapper.Map(dto, vaccine);

            try
            {
                await _unitOfWork.GetRepository<Vaccine>().UpdateAsync(vaccine);
                await _unitOfWork.GetRepository<Vaccine>().SaveAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed update vaccine to db: " + e.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Vaccine? vaccine = await _unitOfWork.GetRepository<Vaccine>().GetByIdAsync(id);
            if (vaccine == null) return false;
            if (await _unitOfWork.GetRepository<PackageVaccine>().Entities.AnyAsync(pv => pv.VaccineId == id))
                throw new Exception("Vaccine is being existed in a package! Can't delete it");

            await _unitOfWork.GetRepository<Vaccine>().DeleteAsync(vaccine);
            await _unitOfWork.GetRepository<Vaccine>().SaveAsync();
            return true;
        }
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            Vaccine? vaccine = await _unitOfWork.GetRepository<Vaccine>().GetByIdAsync(id);
            if (vaccine == null) return false;

            vaccine.Status = 0;
            vaccine.DeletedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Vaccine>().UpdateAsync(vaccine);
            await _unitOfWork.GetRepository<Vaccine>().SaveAsync();
            return true;
        }

        public async Task<IEnumerable<VaccineGetDto>> GetVaccineByPackageId(Guid packageId)
        {
            if (packageId == Guid.Empty) return null;
            IEnumerable<Guid> vaccineIds = await _unitOfWork.GetRepository<PackageVaccine>()
                .Entities
                .Where(pv => pv.PackageId == packageId)
                .Select(pv => pv.VaccineId)
                .ToListAsync();

            List<Vaccine> vaccines = new();
            foreach(var vaccineId in vaccineIds)
            {
                vaccines.Add(await _unitOfWork.GetRepository<Vaccine>().GetByIdAsync(vaccineId));
            }

            return _mapper.Map<IEnumerable<VaccineGetDto>>(vaccines);
        }
    }
}
