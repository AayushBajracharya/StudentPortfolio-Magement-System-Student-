using Application.Dto.Portfolio;
using Application.Dto.Student;
using Application.Interfaces.Repositories.PortfolioRepository;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence.Services.PortfolioService
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStudentService _studentService;

        public PortfolioService(IPortfolioRepository portfolioRepository, IStudentService studentService, IWebHostEnvironment webHostEnvironment)
        {
            _portfolioRepository = portfolioRepository;
            _studentService = studentService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<int> CreatePortfolioAsync(CreatePortfolioDTO portfolioDto)
        {
            var student = await _studentService.GetStudentByIdAsync(portfolioDto.StudentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            // Save the uploaded document (file)
            var fileUrl = await SaveDocumentAsync(portfolioDto.Document);

            var portfolio = new Portfolio
            {
                StudentId = portfolioDto.StudentId,
                Feedback = portfolioDto.Feedback,
                StudentName = student.Name,
                DocumentUrl = fileUrl
            };

            await _portfolioRepository.AddAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            return portfolio.Id;
        }

        public async Task<bool> DeletePortfolioAsync(int portfolioId)
        {
            var portfolio = await _portfolioRepository.FirstOrDefaultAsync(x => x.Id == portfolioId);
            if (portfolio == null) return false;

            await _portfolioRepository.RemoveAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();

            return true;
        }

        public async Task<(IQueryable<PortfolioDTO> portfolios, int totalCount)> GetAllPortfolioAsync(string studentName = null, int? studentId = null, int pageNumber = 1, int pageSize = 5)
        {
            var portfoliosQuery = _portfolioRepository.GetQueryable();

            // Apply filters if provided
            portfoliosQuery = portfoliosQuery
                .Where(s => (studentId == null || s.StudentId == studentId) &&
                            (string.IsNullOrEmpty(studentName) || s.StudentName.ToLower().Contains(studentName)));




            // Get the total count before pagination
            var totalCount = portfoliosQuery.Count();

            // Apply pagination if pageSize is greater than 0
            if (pageSize > 0)
            {
                portfoliosQuery = portfoliosQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            var portDto = portfoliosQuery.Select(portfolio => new PortfolioDTO
            {
                Id = portfolio.Id,
                StudentId = portfolio.StudentId,
                StudentName = portfolio.StudentName,
                Feedback = portfolio.Feedback,
                DocumentUrl = portfolio.DocumentUrl
            });
            return (portDto, totalCount);

        }

        public async Task<bool> UpdatePortfolioAsync(UpdatePortfolioDTO portfolio)
        {
            var student = await _studentService.GetStudentByIdAsync(portfolio.StudentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }

            var details = await _portfolioRepository.FirstOrDefaultAsync(x => x.Id == portfolio.Id);
            if (details == null) return false;

            // Save the uploaded document (file)
            var fileUrl = await SaveDocumentAsync(portfolio.Document);

            details.StudentId = portfolio.StudentId; 
            details.StudentName = student.Name;
            details.Feedback = portfolio.Feedback;
            details.DocumentUrl = fileUrl;


            await _portfolioRepository.UpdateAsync(details);
            await _portfolioRepository.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveDocumentAsync(IFormFile document)
        {
            if (document == null || document.Length == 0)
                throw new Exception("Document cannot be empty");

            var documentFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "documents");
            if (!Directory.Exists(documentFolderPath))
            {
                Directory.CreateDirectory(documentFolderPath);
            }

            var fileName = $"{Guid.NewGuid()}.pdf";
            var filePath = Path.Combine(documentFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await document.CopyToAsync(stream);
            }

            return $"/documents/{fileName}";
        }
    }

}
