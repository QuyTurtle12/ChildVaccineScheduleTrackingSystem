using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;
namespace BusinessLogic.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUOW _unitOfWork;
        private readonly IMapper _mapper;

        public PackageService(IMapper mapper, IUOW unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PackageGetDTO>> GetAllAsync()
        {
            IEnumerable<Package> packages = await _unitOfWork.GetRepository<Package>().GetAllAsync();
            IEnumerable<PackageGetDTO> result = _mapper.Map<IEnumerable<PackageGetDTO>>(packages);
            foreach (var item in result)
            {
                await AssignAppointmentNameToDto(item);
            }
            return result;
        }

        public async Task<PackageGetDTO> GetByIdAsync(Guid id)
        {
            Package? package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(id);

            PackageGetDTO result = _mapper.Map<PackageGetDTO>(package);
            await AssignAppointmentNameToDto(result);
            return result;
        }

        public async Task<PackageGetDTO> CreateAsync(PackagePostDTO dto)
        {
            Package package = _mapper.Map<Package>(dto);
            package.CreatedTime = DateTime.Now;
            package.LastUpdatedTime = package.CreatedTime;

            await _unitOfWork.GetRepository<Package>().InsertAsync(package);
            await _unitOfWork.GetRepository<Package>().SaveAsync();

            PackageGetDTO result = _mapper.Map<PackageGetDTO>(package);
            await AssignAppointmentNameToDto(result);
            return result;
        }

        public async Task<bool> UpdateAsync(Guid id, PackagePutDTO dto)
        {
            Package? package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(id);
            if (package == null) return false;

            _mapper.Map(dto, package);

            await _unitOfWork.GetRepository<Package>().UpdateAsync(package);
            await _unitOfWork.GetRepository<Package>().SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Package? package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(id);
            if (package == null) return false;

            await _unitOfWork.GetRepository<Package>().DeleteAsync(package);
            await _unitOfWork.GetRepository<Package>().SaveAsync();
            return true;
        }
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            Package? package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(id);
            if (package == null) return false;
            package.Status = 0;
            package.DeletedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Package>().UpdateAsync(package);
            await _unitOfWork.GetRepository<Package>().SaveAsync();
            return true;
        }
        private async Task AssignAppointmentNameToDto(PackageGetDTO dto)
        {
            dto.AppointmentName = _unitOfWork.GetRepository<Appointment>()
                .Entities
                .Where(a => a.Id == dto.AppointmentId)
                .Select(a => a.Name)
                .ToString();
        }
    }
}
