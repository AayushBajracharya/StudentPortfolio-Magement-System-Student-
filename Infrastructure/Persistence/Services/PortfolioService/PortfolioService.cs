using Application.Dto.Portfolio;
using Application.Interfaces.Repositories.PortfolioRepository;
using Application.Interfaces.Repositories.StudentRepository;
using Application.Interfaces.Services.PortfolioService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Services.PortfolioService
{
    public class PortfolioService : IPortfolioService
    {

        //private readonly IPortfolioRepository _portfolioRepository;
        //private readonly IStudentRepository _studentRepository;
        //private readonly IWebHostEnvironment _hostingEnvironment;

        //public PortfolioService(IPortfolioRepository portfolioRepository, IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment)
        //{
        //    _studentRepository = studentRepository;

        //    _portfolioRepository = portfolioRepository;
        //    _hostingEnvironment = hostingEnvironment;
        //}

        //public async Task<int> CreatePortfolioAsync(CreatePortfolioDTO portfolioDTO)
        //{
        //    // Step 1: Validate the student exists
        //    // Step 1: Validate the student exists
        //    var student = await _studentRepository.GetStudentByIdAsync(portfolioDTO.StudentId);

        //    if (student == null)
        //        throw new Exception("Student not found");

        //    // Step 2: Create the Portfolio entity from the DTO
        //    var portfolio = new Portfolio
        //    {
        //        StudentId = portfolioDTO.StudentId,
        //        StudentName = portfolioDTO.StudentName ?? student.Name, // Use the student's name if not provided
        //        Documents = portfolioDTO.Documents
        //    };

        //    // Step 3: Add the portfolio to the database
        //    await _portfolioRepository.AddAsync(portfolio);
        //    await _portfolioRepository.SaveChangesAsync();

        //    // Step 4: Return the Portfolio ID (or another relevant identifier)
        //    return portfolio.Id;
        //}
        public Task<int> CreatePortfolioAsync(CreatePortfolioDTO portfolio)
        {
            throw new NotImplementedException();
        }
    }
}
