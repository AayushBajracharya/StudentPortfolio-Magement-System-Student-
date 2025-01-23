using Application.Dto.Portfolio;
using Application.Interfaces.Services.PortfolioService;
using MediatR;

namespace Application.Features.Portfolio.Query
{
    public class GetAllPortfolioQuery : IRequest<(IQueryable<PortfolioDTO>, int)>
    {
        public string StudentName { get; set; }
        public int? StudentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class GetAllPortfolioHandlers : IRequestHandler<GetAllPortfolioQuery, (IQueryable<PortfolioDTO>, int)>
    {
        private readonly IPortfolioService _portfolioService;
        // Dependency Injection
        public GetAllPortfolioHandlers(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<(IQueryable<PortfolioDTO>, int)> Handle(GetAllPortfolioQuery request, CancellationToken cancellationToken)
        {
            return await _portfolioService.GetAllPortfolioAsync(request.StudentName, request.StudentId, request.PageNumber, request.PageSize);
        }
    }
}
