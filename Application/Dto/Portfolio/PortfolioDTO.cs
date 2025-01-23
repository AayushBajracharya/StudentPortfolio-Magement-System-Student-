using Microsoft.AspNetCore.Http;

namespace Application.Dto.Portfolio
{
    public class PortfolioDTO
    {
        public int Id { get; set; } // Unique ID for the portfolio
        public int StudentId { get; set; } // Foreign key linking to the Student model
        public string StudentName { get; set; } // Student's name (optional, but helpful)
        public string Feedback { get; set; }

        public string DocumentUrl { get; set; }  // PDF Document URL
    }
}
