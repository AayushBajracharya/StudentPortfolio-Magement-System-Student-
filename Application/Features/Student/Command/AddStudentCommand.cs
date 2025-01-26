using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Command
{
    public class AddStudentCommand : IRequest<AddStudentDto>
    {
        public AddStudentDto Student { get; set; }
    }

    public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, AddStudentDto>
    {
        private readonly IStudentService _studentService;

        public AddStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<AddStudentDto> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentService.AddStudentAsync(request.Student);
            return student;
        }
    }



}
