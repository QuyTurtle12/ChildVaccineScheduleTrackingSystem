using BusinessLogic.DTOs.AppointmentDTO;
using Data.PaggingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<GetAppointmentDTO>> GetAllAppointments();
        Task<GetAppointmentDTO> GetAppointmentById(Guid id);
        Task<PaginatedList<GetAppointmentDTO>> GetAppointments(int index, int pageSize, Guid? idSearch, string? userSearch, string? nameSearch, DateTimeOffset? fromDateSearch, DateTimeOffset? toDateSearch, int? statusSearch);

        Task<PostAppointmentDTO> CreateAppointment(PostAppointmentDTO appointmentDto);
        Task<PutAppointmentDTO> UpdateAppointment(Guid id, PutAppointmentDTO appointmentDto);
        Task<bool> DeleteAppointment(Guid id);
    }
}
