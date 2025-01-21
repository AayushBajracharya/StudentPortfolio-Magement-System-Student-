namespace Domain.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }  // Portfolio ID
        public int StudentId { get; set; }  // Foreign key to Student
        public string StudentName { get; set; }  // Student's Name
        public string DocumentUrl { get; set; }  // PDF Document URL

        // Navigation property
        public Student Student { get; set; }


    }
}
