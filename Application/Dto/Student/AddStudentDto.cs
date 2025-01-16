using Microsoft.AspNetCore.Http;

namespace Application.Dto.Student
{
    public class AddStudentDto
    {
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Gender { get; set; }
        public DateTime DoB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Faculty { get; set; }
        public string Semester { get; set; }
        public string PhoneNo { get; set; }
        public string ImageUrl { get; set; }
        public List<IFormFile> Images { get; set; }

    }
}
