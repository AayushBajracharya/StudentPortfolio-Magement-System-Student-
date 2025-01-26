using Application.Dto.Portfolio;
using Application.Dto.Student;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
using MediatR;

namespace Application.Features.Portfolio.Command
{
    public class CreatePortfolioCommand : IRequest<CreatePortfolioDTO> // Return type is int (Portfolio ID)
    {
        public CreatePortfolioDTO Portfolio { get; set; }
    }

    public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, CreatePortfolioDTO>
    {
        private readonly IPortfolioService _portfolioService;

        public CreatePortfolioCommandHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task<CreatePortfolioDTO> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
        {

            var porttfolio = await _portfolioService.CreatePortfolioAsync(request.Portfolio);
            return porttfolio;
        }
    }
}
