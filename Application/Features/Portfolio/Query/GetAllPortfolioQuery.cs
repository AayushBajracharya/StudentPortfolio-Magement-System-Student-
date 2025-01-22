using Application.Dto.Portfolio;
using Application.Features.Student.Query;
using Application.Interfaces.Repositories.StudentRepository;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Portfolio.Query
{
    public class GetAllPortfolioQuery : IRequest<IEnumerable<PortfolioDTO>>
    {
    }

    public class GetAllPortfolioHandlers : IRequestHandler<GetAllPortfolioQuery, IEnumerable<PortfolioDTO>>
    {
        private readonly IPortfolioService _portfolioService;
        // Dependency Injection
        public GetAllPortfolioHandlers(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<IEnumerable<PortfolioDTO>> Handle(GetAllPortfolioQuery request, CancellationToken cancellationToken)
        {
            return await _portfolioService.GetAllPortfolioAsync();
        }
    }
}
