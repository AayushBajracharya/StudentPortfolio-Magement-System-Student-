using Application.Dto.Student;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Command
{
    public class UpdateStudentCommand : IRequest<bool>
    {
        public UpdateStudentDto Student { get; set; }
    }

    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public UpdateStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentService.UpdateStudentAsync(request.Student);
            return result;
        }
    }
}
