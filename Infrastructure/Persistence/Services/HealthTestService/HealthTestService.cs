using Application.Dto.Doctor;
using Application.Dto.HealthTest;
using Application.Dto.UploadFile;
using Application.Interfaces.Repositories.HealthTestRepository;
using Application.Interfaces.Repositories.UploadFileMorphRepository;
using Application.Interfaces.Services.HealthTestService;
using Application.Interfaces.Services.UploadFileService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Infrastructure.Persistence.Services.HealthTestService
{
    public class HealthTestService : IHealthTestService
    {
        private readonly ILogger<HealthTestService> _logger;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUploadFileMorphRepository _uploadFileMorphRepository;
        private readonly IHealthTestRepository _healthTestRepository;

        public HealthTestService(ILogger<HealthTestService> logger, IHealthTestRepository healthTestRepository, IUploadFileMorphRepository uploadFileMorphRepository,
            IUploadFileService uploadFileService)
        {
            _logger = logger;
            _uploadFileMorphRepository = uploadFileMorphRepository;
            _uploadFileService = uploadFileService;
            _healthTestRepository = healthTestRepository;
        }

        public async Task<AddHealthTestDto> AddHealthTestAsync(AddHealthTestDto request)
        {
            //_logger.LogInformation($"{nameof(AddHealthTestAsync)} trigger function received a request to new Health Test");
            var NewHealthTest = new HealthTest
            {
                TestName = request.TestName,
                Description = request.Description,
                RequestedDate = request.RequestedDate,
                PatientId = request.PatientId,
              //  Patient = request.Patient,
            };
            await _healthTestRepository.AddAsync(NewHealthTest);
            await _healthTestRepository.SaveChangesAsync();
             _logger.LogInformation($"{nameof(AddHealthTestAsync)} trigger function successfully added a new Health Test");

            if (request.ImageIds != null && request.ImageIds.Any())
            {
                foreach (var imageId in request.ImageIds)
                {
                    var uploadFile = await _uploadFileService.GetUploadFileByIdAsync(imageId);

                    var uploadFileMorph = new UploadFileMorph
                    {
                        UploadFileId = imageId,
                        RelatedId = NewHealthTest.HealthTestId,
                        RelatedType = "healthtest",
                        Field = "images",

                    };
                    _uploadFileMorphRepository.Add(uploadFileMorph);
                }
                await _uploadFileMorphRepository.SaveChangesAsync();
            }
            return request;
        }
       
        public async Task<List<HealthTestDto>> GetAllHealthTestAsync()
        {
            List<HealthTestDto> healthTestDtoList = new List<HealthTestDto>();
            var healthTestList = await _healthTestRepository.Queryable.ToListAsync();

            foreach (var healthTest in healthTestList)
            {
                var healthTestDto = new HealthTestDto
                {
                    HealthTestId = healthTest.HealthTestId,
                    TestName = healthTest.TestName,
                    Description = healthTest.Description,
                    RequestedDate = healthTest.RequestedDate,
                    PatientId = healthTest.PatientId,
                    Images = new List<UploadFileDto>()
                };

                var imagesList = await _uploadFileMorphRepository.Queryable
                    .Where(x => x.RelatedId == healthTest.HealthTestId && x.RelatedType == "healthtest" && x.Field == "images")
                    .Include(m => m.UploadFile)
                    .ToListAsync();

               // var imageDtoList = new List<UploadFileDto>();
                foreach (var img in imagesList)
                {
                    healthTestDto.Images.Add(new UploadFileDto
                    {
                        Id = img.UploadFile.Id,
                        Name = img.UploadFile.Name,
                        AlternativeText = img.UploadFile.AlternativeText,
                        Url = img.UploadFile.Url,
                        Ext = img.UploadFile.Ext,
                        Size = img.UploadFile.Size,
                        Mime = img.UploadFile.Mime,
                        Hash = img.UploadFile.Hash,
                    });
                }

                healthTestDtoList.Add(healthTestDto);
            }

            return healthTestDtoList;
        }
        /*
        public async Task <HealthTestDto> GetHealthTestByIdAsync(int id)
        {
            List<HealthTestDto> healthTestDtoList = new List<HealthTestDto>();
            var healthTestList = await _healthTestRepository.Queryable.ToListAsync();

            foreach (var healthTest in healthTestList)
            {
                var healthTestDto = new HealthTestDto
                {
                    HealthTestId = healthTest.HealthTestId,
                    TestName = healthTest.TestName,
                    Description = healthTest.Description,
                    RequestedDate = DateTime.Now,
                    PatientId = healthTest.PatientId,
                    Images = new List<UploadFileDto>()
                };

                var imagesList = await _uploadFileMorphRepository.Queryable
                    .Where(x => x.RelatedId == healthTest.HealthTestId && x.RelatedType == "healthtests" && x.Field == "images")
                    .Include(m => m.UploadFile)
                    .ToListAsync();

                foreach (var img in imagesList)
                {
                    healthTestDto.Images.Add(new UploadFileDto
                    {
                        Id = img.UploadFile.Id,
                        Name = img.UploadFile.Name,
                        AlternativeText = img.UploadFile.AlternativeText,
                        Url = img.UploadFile.Url,
                        Ext = img.UploadFile.Ext,
                        Size = img.UploadFile.Size,
                        Mime = img.UploadFile.Mime,
                        Hash = img.UploadFile.Hash,
                    });
                }

                healthTestDtoList.Add(healthTestDto);
            }

            return healthTestDtoList;
        }
        /*
        public async Task<HealthTestDto> UpdateHealthTestAsync(HealthTestDto request)
        {
            _logger.LogInformation($"{nameof(UpdateHealthTestAsync)} trigger function received a request to update healthtest with Id: {request.HealthTestId}");

            var healthTest = await _healthTestRepository.Queryable.FirstOrDefaultAsync(b => b.HealthTestId == request.HealthTestId);
            if (healthTest == null)
            {
                _logger.LogWarning($"HealthTest with Id {request.HealthTestId} not found.");
                return null;
            }

            healthTest.HealthTestId = request.HealthTestId;
            healthTest.TestName = request.TestName;
            healthTest.Description = request.Description;
            healthTest.RequestedDate = DateTime.Now;
            healthTest.PatientId = request.PatientId;
          //  healthTest.Patient = request.Patient;

            _healthTestRepository.Update(healthTest);
            await _healthTestRepository.SaveChangesAsync();

            _logger.LogInformation($"{nameof(UpdateHealthTestAsync)} trigger function successfully updated the existing Healthtest");

            return request;
        }
        */
    }
    /*
       public async Task<int> DeleteHealthTestAsync(int id)
       {
           _logger.LogInformation($"{nameof(DeleteHealthTestAsync)} trigger function received a request to delete an HealthTest with Id: {id}");

           var HealthTestToDelete = await _healthTestRepository.Queryable.FirstOrDefaultAsync(b => b.HealthTestId== id);

           var healthTest = new HealthTestDto
           {
               HealthTestId = HealthTestToDelete.HealthTestId,
           };

           _healthTestRepository.Remove(HealthTestToDelete);

           await _healthTestRepository.SaveChangesAsync();

           _logger.LogInformation($"{nameof(DeleteHealthTestAsync)} trigger function successfully deleted the HealthTest with Id: {id}");

           return healthTest.HealthTestId;
       }

       */
}
