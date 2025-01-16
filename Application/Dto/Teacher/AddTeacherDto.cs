using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Teacher
{
    public class AddTeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Phonenumber { get; set; }
        public string ImageUrl { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
