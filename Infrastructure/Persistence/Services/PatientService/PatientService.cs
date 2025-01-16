using Application.Dto.Patient;
using Application.Interfaces.Repositories.PatientRepository;
using Application.Interfaces.Services.PatientService;
using System.Text;

namespace Infrastructure.Persistence.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService( IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> Register(PatientDto request)
        {
            var patient = new Patient
            {
                Name = request.Name,
                EmailAddress = request.EmailAddress,
                Password = HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
            };

            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return request;
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
