using Application.Dto.Patient;
using Application.Interfaces.Services.PatientService;
using MediatR;

namespace Application.Features.Patient.Command
{
    public class PatientRegisterCommand : PatientDto, IRequest<PatientDto>
    {

    }

    public class PatientRegisterCommandHandler : IRequestHandler<PatientRegisterCommand, PatientDto>
    {
        private readonly IPatientService _patientService;
        public PatientRegisterCommandHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<PatientDto> Handle(PatientRegisterCommand request, CancellationToken cancellationToken)
        {
            var response = await _patientService.Register(request);
            return response;
        }
    }
}
