using Application.Dto.UploadFile;
using Application.Interfaces.Services.UploadFileService;
using MediatR;


namespace Application.Features.UploadFile.Command.DeleteUploadFileCommand
{
    public class DeleteUploadFileCommand : UploadFileDto, IRequest<int>
    {

    }

    public class DeleteUploadFileCommandHandler : IRequestHandler<DeleteUploadFileCommand, int>
    {
        private readonly IUploadFileService _uploadFileService;

        public DeleteUploadFileCommandHandler(IUploadFileService uploadFileService)
        {
            this._uploadFileService = uploadFileService;
        }

        public async Task<int> Handle(DeleteUploadFileCommand request, CancellationToken cancellationToken)
        {

            var response = await _uploadFileService.DeleteUploadFile(request.Id);

            return response.Id;
        }

    }
}
