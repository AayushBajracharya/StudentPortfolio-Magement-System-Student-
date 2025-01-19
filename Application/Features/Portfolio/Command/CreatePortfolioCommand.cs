using Application.Dto.Portfolio;
using Application.Dto.Student;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Portfolio.Command
{
    public class CreatePortfolioCommand : IRequest<int> // Return type is int (Portfolio ID)
    {
        public CreatePortfolioDTO Portfolio { get; set; }
    }

    public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, int>
    {
        private readonly IPortfolioService _portfolioService;

        public CreatePortfolioCommandHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<int> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
        {

            var porttfolioId = await _portfolioService.CreatePortfolioAsync(request.Portfolio);
            return porttfolioId;
        }
    }
}
