using Application.Interfaces.Repositories.HealthTestRepository;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories.HealthTestRepository
{
    public class HealthTestRepository : Repository<HealthTest>, IHealthTestRepository
    {
        //
        public HealthTestRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
