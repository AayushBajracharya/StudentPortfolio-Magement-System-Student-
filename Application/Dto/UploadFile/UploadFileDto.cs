

using Microsoft.AspNetCore.Http;

namespace Application.Dto.UploadFile
{
    public class UploadFileDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AlternativeText { get; set; }

        public string Url { get; set; }
        public string Hash { get; set; } 

        public string Ext { get; set; } 

        public string Mime { get; set; }

        public double Size { get; set; }
        //public IFormFile Images { get; set; }
    }
}
