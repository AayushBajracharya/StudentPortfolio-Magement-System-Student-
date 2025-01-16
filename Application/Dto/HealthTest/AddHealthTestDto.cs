
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.HealthTest
{
    public class AddHealthTestDto
    {
        public string TestName { get; set; }
        public string Description { get; set; }
        public DateTime RequestedDate { get; set; }
        public int PatientId { get; set; }
        public List<int> ImageIds { get; set; }
    }
}
