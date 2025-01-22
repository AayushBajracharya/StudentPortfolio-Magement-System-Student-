using Application.Dto.Portfolio;
using Application.Dto.Student;
using Application.Features.Student.Command;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
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
