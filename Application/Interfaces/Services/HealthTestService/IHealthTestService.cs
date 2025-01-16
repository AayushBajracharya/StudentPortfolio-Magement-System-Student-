using Application.Dto.HealthTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.HealthTestService
{
    public interface IHealthTestService
    {
       // public Task<HealthTestDto> GetHealthTestByIdAsync(int id);
        public Task<List<HealthTestDto>> GetAllHealthTestAsync();
        public Task<AddHealthTestDto> AddHealthTestAsync(AddHealthTestDto request);
      //  public Task<HealthTestDto> UpdateHealthTestAsync(HealthTestDto request);
      //  public Task<int> DeleteHealthTestAsync(int id);
    }
}
