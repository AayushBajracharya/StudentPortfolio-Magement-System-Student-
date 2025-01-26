using Application.Dto.Portfolio;
using Application.Interfaces.Services.PortfolioService;
using MediatR;

namespace Application.Features.Portfolio.Command
{
    public class UpdatePortfolioCommand : IRequest<bool>
    {
        public UpdatePortfolioDTO Portfolio { get; set; }
    }

    public class UpdatePortfolioCommandHandler : IRequestHandler<UpdatePortfolioCommand, bool>
    {
        private readonly IPortfolioService _portfolioService;

        public UpdatePortfolioCommandHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<bool> Handle(UpdatePortfolioCommand request, CancellationToken cancellationToken)
        {
            var result = await _portfolioService.UpdatePortfolioAsync(request.Portfolio);
            return result;
        }
    }
}
