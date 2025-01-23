using Application.Dto.Portfolio;
using Application.Dto.Student;
using Domain.Entities;

namespace Application.Interfaces.Services.PortfolioService
{
    public interface IPortfolioService
    {
        Task<int> CreatePortfolioAsync(CreatePortfolioDTO portfolio);
        //Task<PortfolioDTO> GetPortfolioByIdAsync(int portfolioId);
        //Task<IQueryable<PortfolioDTO>> GetAllPortfolioAsync();

        Task<(IQueryable<PortfolioDTO> portfolios, int totalCount)> GetAllPortfolioAsync(
            string studentName = null,
            int? studentId = null,
            int pageNumber = 1,
            int pageSize = 5);

        Task<bool> UpdatePortfolioAsync(UpdatePortfolioDTO portfolio);
        Task<bool> DeletePortfolioAsync(int portfolioId);
    }
}
