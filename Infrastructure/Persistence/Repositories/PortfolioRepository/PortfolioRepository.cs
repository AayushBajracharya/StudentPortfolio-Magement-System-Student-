using Application.Interfaces.Repositories.PortfolioRepository;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories.PortfolioRepository
{
    public class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
