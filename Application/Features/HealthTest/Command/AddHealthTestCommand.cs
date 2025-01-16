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

namespace Application.Features.HealthTest.Command
{
    public class AddHealthTestCommand : AddHealthTestDto, IRequest<AddHealthTestDto>
    {
    }

    public class AddHealthTestCommandHandler : IRequestHandler<AddHealthTestCommand, AddHealthTestDto>
    {
        private readonly IHealthTestService _healthTestService;

        public AddHealthTestCommandHandler(IHealthTestService healthTestService)
        {
            _healthTestService = healthTestService;
        }

        public async Task<AddHealthTestDto> Handle(AddHealthTestCommand request, CancellationToken cancellationToken)
        {
            var response = await _healthTestService.AddHealthTestAsync(request);
            return response;
        }
    }
}
