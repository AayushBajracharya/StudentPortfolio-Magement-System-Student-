using Application.Dto.Teacher;
using Application.Interfaces.Services.TeacherService;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Teacher.Command
{
    public class TeacherRegistrationCommand : IRequest<AddTeacherDto>
    {
        public AddTeacherDto AddTeacherDto { get; set; }
    }

    public class TeacherRegistrationCommandHandler : IRequestHandler<TeacherRegistrationCommand, AddTeacherDto>
    {
        private readonly ITeacherService _teacherService;

        public TeacherRegistrationCommandHandler(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public async Task<AddTeacherDto> Handle(TeacherRegistrationCommand request, CancellationToken cancellationToken)
        {
            // Save teacher with images using the service
            var response = await _teacherService.RegisterTeacher(request.AddTeacherDto);
            return response;
        }
    }
}
