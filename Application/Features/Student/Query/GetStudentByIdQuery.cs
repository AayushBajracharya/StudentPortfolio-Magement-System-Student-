using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Query
{
    public class GetStudentByIdQuery : IRequest<StudentDto>
    {
        public int Id { get; set; }
    }

    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        private readonly IStudentService _studentService;

        public GetStudentByIdQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _studentService.GetStudentByIdAsync(request.Id);
        }
    }
}
