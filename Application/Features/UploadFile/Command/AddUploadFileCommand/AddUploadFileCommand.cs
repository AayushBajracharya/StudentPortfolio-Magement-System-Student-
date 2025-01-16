using Application.Dto.UploadFile;
using Application.Interfaces.Services.UploadFileService;
using MediatR;

namespace Application.Features.UploadFile.Command.AddUploadFileCommand
{
    public class AddUploadFileCommand : AddUploadFileDto, IRequest<AddUploadFileDto>
    {

    }

    public class AddUploadFileCommandHandler : IRequestHandler<AddUploadFileCommand, AddUploadFileDto>
    {
        private readonly IUploadFileService _uploadFileService;

        public AddUploadFileCommandHandler(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        public async Task<AddUploadFileDto> Handle(AddUploadFileCommand request, CancellationToken cancellationToken)
        {
            var response = await _uploadFileService.AddUploadFile(request);

            return response;
        }
    }
}
