
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment
    {

        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // e.g., Available, scheduled, canceled, completed
        [MaxLength(250)]
        public string? Notes { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}
