using Application.Dto.Doctor;
using Application.Dto.HealthTest;
using Application.Interfaces.Services.DoctorService;
using Application.Interfaces.Services.HealthTestService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.HealthTest.Query
{
    public class HealthTestListQuery : IRequest<List<HealthTestDto>>
    {

    }
    public class HealthTestListQueryHandler : IRequestHandler<HealthTestListQuery, List<HealthTestDto>>
    {
        private readonly IHealthTestService _healthTestService;
        public HealthTestListQueryHandler(IHealthTestService healthTestService)
        {
            _healthTestService = healthTestService;
        }
        public async Task<List<HealthTestDto>> Handle(HealthTestListQuery request, CancellationToken cancellationToken)
        {
            var response = await _healthTestService.GetAllHealthTestAsync();
            return response;
        }
    }
}
