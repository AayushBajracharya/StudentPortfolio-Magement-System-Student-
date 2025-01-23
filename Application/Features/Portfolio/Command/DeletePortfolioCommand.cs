using Application.Interfaces.Services.PortfolioService;
using MediatR;

namespace Application.Features.Portfolio.Command
{
    public class DeletePortfolioCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeletePortfolioCommandHandler : IRequestHandler<DeletePortfolioCommand, bool>
    {
        private readonly IPortfolioService _portfolioService;

        public DeletePortfolioCommandHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<bool> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            var result = await _portfolioService.DeletePortfolioAsync(request.Id);
            return result;
        }
    }
}
