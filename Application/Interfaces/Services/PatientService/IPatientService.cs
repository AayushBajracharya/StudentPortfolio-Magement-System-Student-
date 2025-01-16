

using Application.Dto.Patient;

namespace Application.Interfaces.Services.PatientService
{
    public interface IPatientService
    {
        Task<PatientDto> Register(PatientDto Data);
    }
}
