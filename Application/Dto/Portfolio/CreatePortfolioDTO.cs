using Microsoft.AspNetCore.Http;

namespace Application.Dto.Portfolio
{
    public class CreatePortfolioDTO
    {
        public int StudentId { get; set; }  // Foreign key to Student
        public string StudentName { get; set; }  // Student's Name

        // The Document property will represent the uploaded file
        public string Feedback { get; set; }

        public IFormFile Document { get; set; }  // PDF Document (IFormFile for file uploads)
    }
}
