using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Command
{
    public class AddStudentCommand : IRequest<int>
    {
        public AddStudentDto Student { get; set; }
    }

    public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, int>
    {
        private readonly IStudentService _studentService;

        public AddStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<int> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var studentId = await _studentService.AddStudentAsync(request.Student);
            return studentId;
        }
    }



}
