using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Interface;
using Microsoft.EntityFrameworkCore;

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
            IEnumerable<Package> packages = await _unitOfWork.GetRepository<Package>()
                .Entities
                .OrderByDescending(x => x.CreatedTime)
                .ToListAsync(); 
            
            return _mapper.Map<IEnumerable<PackageGetDTO>>(packages);
        }

        public async Task<PackageGetDTO> GetByIdAsync(Guid id)
        {
            Package? package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(id);

            PackageGetDTO result = _mapper.Map<PackageGetDTO>(package);
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

            if (await _unitOfWork.GetRepository<PackageVaccine>().Entities.AnyAsync(pv => pv.PackageId == id))
            {
                await UpdatePackageVaccines(id, new List<Guid>());
            }
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

        public async Task UpdatePackageVaccines(Guid packageId, List<Guid> newVaccineIds)
        {
            if (packageId == Guid.Empty)
            {
                throw new Exception("Invalid package id");
            }

            var repo = _unitOfWork.GetRepository<PackageVaccine>();

            // Get current vaccine IDs in the package
            var existingVaccineIds = await repo.Entities
                .Where(pv => pv.PackageId == packageId)
                .Select(pv => pv.VaccineId)
                .ToListAsync();

            // Find vaccines to add (new ones that don't exist in current)
            var toAdd = newVaccineIds.Except(existingVaccineIds)
                .Select(vaccineId => new PackageVaccine { PackageId = packageId, VaccineId = vaccineId })
                .ToList();

            // Find vaccines to remove (existing ones that are not in new list)
            var toRemove = existingVaccineIds.Except(newVaccineIds)
                .ToList();

            // Insert new vaccines
            if (toAdd.Any())
            {
                await repo.InsertRangeAsync(toAdd);
            }

            // Delete removed vaccines
            if (toRemove.Any())
            {
                var deleteEntities = await repo.Entities
                    .Where(pv => pv.PackageId == packageId && toRemove.Contains(pv.VaccineId))
                    .ToListAsync();

                await repo.DeleteRangeAsync(deleteEntities);
            }

            // Save changes once
            await _unitOfWork.SaveAsync();
        }
    }
}
