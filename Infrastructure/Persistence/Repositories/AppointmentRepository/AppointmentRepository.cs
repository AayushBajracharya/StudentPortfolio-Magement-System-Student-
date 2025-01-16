

using Application.Interfaces.Repositories.AppointmentRepository;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.AppointmentRepository
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context) 
        {
        
        }
    }
    
}
