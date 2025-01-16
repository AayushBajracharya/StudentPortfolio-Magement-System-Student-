using Application.Dto.UploadFile;
using Application.Interfaces.Services.UploadFileService;
using MediatR;

namespace Application.Features.UploadFile.Command.UpdateUploadFileCommand
{
    public class UpdateUploadFileCommand : AddUploadFileDto, IRequest<int>
    {

    }

    public class UpdateUploadFileCommandHandler : IRequestHandler<UpdateUploadFileCommand, int>
    {
        private readonly IUploadFileService _uploadFileService;

        public UpdateUploadFileCommandHandler(IUploadFileService uploadFileService)
        {
            this._uploadFileService = uploadFileService;
        }

        public async Task<int> Handle(UpdateUploadFileCommand request, CancellationToken cancellationToken)
        {
            var response = await _uploadFileService.UpdateUploadFile(request);
            return response;
        }
    }
}
