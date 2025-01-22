using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Query
{
    public class GetAllStudentsQuery : IRequest<(IQueryable<StudentDto>, int)>
    {
        public string Faculty { get; set; }
        public string Semester { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 0;
    }



    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, (IQueryable<StudentDto>, int)>
    {
        private readonly IStudentService _studentService;

        public GetAllStudentsQueryHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<(IQueryable<StudentDto>, int)> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            return await _studentService.GetAllStudentAsync(request.Faculty, request.Semester, request.Name, request.PageNumber, request.PageSize);
        }
    }

}
