using Application.Dto.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> AddAsync(AppointmentDto request);
        //Task<bool> CheckAvailability(int doctorId,DateTime date, DateTime timeStart);
    }
}
