using Application.Interfaces.Repositories.CustomerMessageRepository;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories.CustomerMessageRepository
{
    public class CustomerMessageRepository : Repository<CustomerMessage>, ICustomerMessageRepository
    {
        public CustomerMessageRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
