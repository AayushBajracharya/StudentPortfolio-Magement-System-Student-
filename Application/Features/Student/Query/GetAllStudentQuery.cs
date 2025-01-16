using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Query
{
    public class GetAllStudentsQuery : IRequest<IQueryable<StudentDto>>
    {
        public string Faculty { get; set; }
        public string Semester { get; set; }
    }

    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IQueryable<StudentDto>>
    {
        private readonly IStudentService _studentService;

        public GetAllStudentsQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IQueryable<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            return await _studentService.GetAllStudentAsync(request.Faculty, request.Semester);
        }
    }
}
