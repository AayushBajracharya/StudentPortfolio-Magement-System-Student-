using Application.Dto.Appointment;
using Application.Interfaces.Repositories.AppointmentRepository;
using Application.Interfaces.Services.AppointmentService;
using System.Globalization;

namespace Infrastructure.Persistence.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<AppointmentDto> AddAsync(AppointmentDto request)
        {
            var appointment = new Appointment
            {
                AppointmentDateTime = request.Date,
                TimeStart = request.TimeStart.TimeOfDay,
                TimeEnd = request.TimeEnd.TimeOfDay,
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                Status = request.Status,
                Notes = request.Notes,
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return request;
        }

        //public async Task<bool> CheckAvailability(int doctorId, DateTime date, DateTime timeStart)
        //=> !await this._appointmentRepository
        //           .AnyAsync(a => a.DoctorId == doctorId
        //                && a.AppointmentDateTime == date
        //                && a.TimeStart <= timeStart && a.TimeEnd < timeStart);

    }
}
