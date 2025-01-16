
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
  
        [Required]
        public string Specialty { get; set; }
    }
}
