using Application.Interfaces.Repositories.DoctorRepository;
using Infrastructure.Persistence.Contexts;


namespace Infrastructure.Persistence.Repositories.DoctorRepository
{
    public class DoctorRepository :  Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context) 
        {
        
        }
    }
}
