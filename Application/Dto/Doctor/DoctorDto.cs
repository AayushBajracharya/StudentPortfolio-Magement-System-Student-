

using Application.Dto.UploadFile;

namespace Application.Dto.Doctor
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public List<UploadFileDto> Images { get; set; } = new List<UploadFileDto>();
    }
}
