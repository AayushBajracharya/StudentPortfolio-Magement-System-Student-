using Application.Dto.Teacher;
using Application.Interfaces.Services.TeacherService;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Teacher.Query
{
    public class GetTeacherByIdQuery : IRequest<TeacherDto>
    {
        public int Id { get; set; }
    }

    public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, TeacherDto>
    {
        private readonly ITeacherService _teacherService;

        public GetTeacherByIdQueryHandler(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public async Task<TeacherDto> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _teacherService.GetTeacherDetails(request.Id);
            return response;
        }
    }
}
