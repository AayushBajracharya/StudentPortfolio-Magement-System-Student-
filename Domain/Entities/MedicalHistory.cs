
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MedicalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime DateOfEntry { get; set; }

        [MaxLength(255)]
        [Required]
        public string MedicalCondition { get; set; }
        [MaxLength(255)]
        public string Medications { get; set; }
        [MaxLength(255)]
        public string Allergies { get; set; }
        [MaxLength(255)]
        public string Surgeries { get; set; }
        [MaxLength(255)]
        public string FamilyMedicalHistory { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
