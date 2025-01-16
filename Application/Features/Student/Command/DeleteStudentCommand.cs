using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Student.Command
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentService _studentService;

        public DeleteStudentCommandHandler(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var result = await _studentService.DeleteStudentAsync(request.Id);
            return result;
        }
    }
}
