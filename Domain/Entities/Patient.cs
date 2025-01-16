

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
    }
}
