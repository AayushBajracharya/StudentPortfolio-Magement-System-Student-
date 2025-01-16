using Application.Dto.Doctor;
using Application.Interfaces.Services.DoctorService;
using MediatR;

namespace Application.Features.Doctor.Query.SearchDoctor
{
    public class SearchDoctorQuery : IRequest<List<DoctorDto>>
    {
        public string searchTerm { get; set; }
    }

    public class SearchDoctorQueryHandler : IRequestHandler<SearchDoctorQuery, List<DoctorDto>>
    {
        private readonly IDoctorService _doctorService;
        public SearchDoctorQueryHandler(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        public async Task<List<DoctorDto>> Handle(SearchDoctorQuery request, CancellationToken cancellationToken)
        {
            var response = await _doctorService.SearchDoctorsAsync(request.searchTerm);
            return response;
        }
    }
}
