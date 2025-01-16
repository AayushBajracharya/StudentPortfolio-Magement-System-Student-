using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class HealthTest
    {
        [Key]
        public int HealthTestId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TestName { get; set; } 

        [MaxLength(500)]
        public string Description { get; set; }  
        public DateTime RequestedDate { get; set; }  
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
