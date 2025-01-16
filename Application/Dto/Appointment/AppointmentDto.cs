

using Application.Converters;
using System.Text.Json.Serialization;

namespace Application.Dto.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonConverter(typeof(TimeOnlyConverter))]
        public DateTime TimeStart { get; set; }

        [JsonConverter(typeof(TimeOnlyConverter))]
        public DateTime TimeEnd { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
