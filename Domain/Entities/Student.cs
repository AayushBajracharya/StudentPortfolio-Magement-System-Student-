namespace Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
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

        //// Navigation property for portfolios
        //public ICollection<Portfolio> Portfolios { get; set; }

        // Navigation property for the related Portfolio (One-to-One relationship)
        public Portfolio Portfolios { get; set; } // A student has exactly one portfolio

    }
}
