

using Application.Dto.Doctor;

namespace Application.Interfaces.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllDoctorAsync();
        Task<List<DoctorDto>> SearchDoctorsAsync(string searchTerm);
        Task<AddDoctorDto> AddDoctorAsync(AddDoctorDto request);
    }
}
