using Application.Dto.Portfolio;
using Domain.Entities;

namespace Application.Interfaces.Services.PortfolioService
{
    public interface IPortfolioService
    {
        Task<int> CreatePortfolioAsync(CreatePortfolioDTO portfolio);
        //Task<PortfolioDTO> GetPortfolioByIdAsync(int portfolioId);
        //Task<IEnumerable<PortfolioDTO>> GetPortfoliosByStudentIdAsync(int studentId);
        //Task<bool> UpdatePortfolioAsync(UpdatePortfolioDTO portfolio);
        //Task<bool> DeletePortfolioAsync(int portfolioId);
    }
}
