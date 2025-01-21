using Application.Dto.Portfolio;
using Application.Interfaces.Repositories.PortfolioRepository;
using Application.Interfaces.Services.PortfolioService;
using Application.Interfaces.Services.StudentService;
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
                StudentName = student.Name,
                DocumentUrl = fileUrl
            };

            await _portfolioRepository.AddAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            return portfolio.Id;
        }

        public Task<bool> UpdatePortfolioAsync(UpdatePortfolioDTO portfolio)
        {
            throw new NotImplementedException();
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
