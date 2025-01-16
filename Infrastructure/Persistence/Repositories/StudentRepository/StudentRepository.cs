using Application.Interfaces.Repositories.StudentRepository;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.StudentRepository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
