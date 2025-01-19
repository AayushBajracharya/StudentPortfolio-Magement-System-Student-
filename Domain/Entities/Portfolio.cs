namespace Domain.Entities
{
    public class Portfolio
    {
        public int Id { get; set; } // Unique ID for the portfolio
        public int StudentId { get; set; } // Foreign key linking to the Student model
        public string StudentName { get; set; } // Student's name (optional, but helpful)
        //public List<string> Documents { get; set; } // List of portfolio documents (e.g., URLs)

        // Navigation property for relationship
        public Student Student { get; set; } // Reference to the associated Student
    }
}
