using Application.Dto.Appointment;
using Application.Interfaces.Services.AppointmentService;
using MediatR;

namespace Application.Features.Appointment.Command.AddAppointmentCommand
{
    public class AddAppointmentCommand : AppointmentDto, IRequest<AppointmentDto>
    {
    }

    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, AppointmentDto>
    {
        private readonly IAppointmentService _appointmentService;

        public AddAppointmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<AppointmentDto> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            var response = await _appointmentService.AddAsync(request);
            return response;
        }
    }
}
