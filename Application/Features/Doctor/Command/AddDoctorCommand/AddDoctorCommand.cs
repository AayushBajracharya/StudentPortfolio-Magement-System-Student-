using Application.Dto.Doctor;
using Application.Interfaces.Services.DoctorService;
using MediatR;

namespace Application.Features.Doctor.Command.AddDoctorCommand
{
    public class AddDoctorCommand : AddDoctorDto, IRequest<AddDoctorDto>
    {
    }

    public class AddDoctorCommandHandler : IRequestHandler<AddDoctorCommand, AddDoctorDto> 
    {
        private readonly IDoctorService _doctorService;

        public AddDoctorCommandHandler(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<AddDoctorDto> Handle(AddDoctorCommand request, CancellationToken cancellationToken)
        {
            var response = await _doctorService.AddDoctorAsync(request);
            return response;
        }
    }
}
