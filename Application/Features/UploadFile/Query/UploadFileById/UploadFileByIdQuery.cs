using Application.Dto.UploadFile;
using Application.Interfaces.Services.UploadFileService;
using MediatR;


namespace Application.Features.UploadFile.Query.UploadFileById
{
    public class UploadFileByIdQuery : IRequest<UploadFileDto>
    {
        public int Id { get; set; }
    }

    public class UploadFileByIdQueryHandler : IRequestHandler<UploadFileByIdQuery, UploadFileDto>
    {
        private readonly IUploadFileService _uploadFileService;

        public UploadFileByIdQueryHandler(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        public async Task<UploadFileDto> Handle(UploadFileByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _uploadFileService.GetUploadFileByIdAsync(request.Id);
            return response;
        }
    }
}
