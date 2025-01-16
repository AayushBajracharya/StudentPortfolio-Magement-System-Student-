using Application.Dto.UploadFile;
using Application.Interfaces.Services.UploadFileService;
using MediatR;


namespace Application.Features.UploadFile.Query.UploadFileList
{
    public class UploadFileListQuery : IRequest<List<UploadFileDto>>
    {

    }

    public class UploadFileListQueryHandler : IRequestHandler<UploadFileListQuery, List<UploadFileDto>>
    {
        private readonly IUploadFileService _uploadFileService;
        public UploadFileListQueryHandler(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }
        public async Task<List<UploadFileDto>> Handle(UploadFileListQuery request, CancellationToken cancellationToken)
        {
            var response = await _uploadFileService.GetAllUploadFileAsync();
            return response;
        }

    }
}
