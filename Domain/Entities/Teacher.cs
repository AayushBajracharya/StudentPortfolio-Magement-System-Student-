using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set;  }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Phonenumber { get; set; }
        public string ImageUrl { get; set; }

    }
}
