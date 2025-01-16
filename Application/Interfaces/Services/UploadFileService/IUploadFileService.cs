

using Application.Dto.UploadFile;

namespace Application.Interfaces.Services.UploadFileService
{
    public interface IUploadFileService
    {
        public Task<UploadFileDto> GetUploadFileByIdAsync(int id);
        public Task<List<UploadFileDto>> GetAllUploadFileAsync();
        public Task<AddUploadFileDto> AddUploadFile(AddUploadFileDto request);
        public Task<int> UpdateUploadFile(AddUploadFileDto request);
        public Task<UploadFileDto> DeleteUploadFile(int id);
    }
}
