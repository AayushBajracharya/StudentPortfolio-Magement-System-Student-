using Microsoft.AspNetCore.Http;

namespace Application.Dto.UploadFile
{
    public class AddUploadFileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string AlternativeText { get; set; }

        public IFormFile Images { get; set; }
    }
}
