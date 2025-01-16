using Application.Dto.Doctor;
using Application.Interfaces.Services.DoctorService;
using MediatR;

namespace Application.Features.Doctor.Query.DoctorList
{
    public class DoctorListQuery : IRequest<List<DoctorDto>>
    {

    }
    public class DoctorListQueryHandler : IRequestHandler<DoctorListQuery, List<DoctorDto>>
    {
        private readonly IDoctorService _doctorService;
        public DoctorListQueryHandler(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        public async Task<List<DoctorDto>> Handle(DoctorListQuery request, CancellationToken cancellationToken)
        {
            var response = await _doctorService.GetAllDoctorAsync();
            return response;
        }
    }
}
