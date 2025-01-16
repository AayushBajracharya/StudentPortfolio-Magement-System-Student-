
using Application.Interfaces.Repositories.PatientRepository;
using Infrastructure.Persistence.Contexts;


namespace Infrastructure.Persistence.Repositories.PatientRepository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context) 
        { 
        
        }
    }
}
