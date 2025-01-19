namespace Application.Dto.Portfolio
{
    public class CreatePortfolioDTO
    {
        public int StudentId { get; set; } // Foreign key linking to the Student model
        public string StudentName { get; set; } // Student's name (optional, but helpful)
        public List<string> Documents { get; set; } // List of portfolio documents (e.g., URLs)
    }
}
