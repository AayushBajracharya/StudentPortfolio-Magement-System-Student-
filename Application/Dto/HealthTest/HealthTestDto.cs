
using Application.Dto.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.HealthTest
{
    public class HealthTestDto
    {
        public int HealthTestId { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }
        public DateTime RequestedDate { get; set; }
        public int PatientId { get; set; }
        public List<UploadFileDto> Images { get; set; } = new List<UploadFileDto>();
    }
}
