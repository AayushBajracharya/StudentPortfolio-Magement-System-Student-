

namespace Application.Dto.Doctor
{
    public class AddDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public List<int> ImageIds { get; set; }
    }
}
