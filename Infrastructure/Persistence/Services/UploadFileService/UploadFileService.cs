using Application.Dto.UploadFile;
using Application.Interfaces.Repositories.UploadFileRepository;
using Application.Interfaces.Services.UploadFileService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Services.UploadFileService
{
    public class UploadFileService : IUploadFileService
    {
        private ILogger<UploadFileService> _logger;
        private readonly IUploadFileRepository _uploadFileRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadFileService(ILogger<UploadFileService> logger, 
            IUploadFileRepository uploadFileRepository,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _uploadFileRepository = uploadFileRepository;
            _config = configuration;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UploadFileDto>> GetAllUploadFileAsync()
        {

            _logger.LogInformation($"{nameof(GetAllUploadFileAsync)} trigger function received a request");

            var uploadFileList = await _uploadFileRepository.Queryable
                                                    .OrderByDescending(uf => uf.Id)
                                                    .ToListAsync();

            List<UploadFileDto> SupplierLists = uploadFileList.Select(uf => new UploadFileDto
            {
                Id = uf.Id,
                Name = uf.Name,
                AlternativeText = uf.AlternativeText,
                Url = uf.Url,
                Hash = uf.Hash,
                Mime = uf.Mime,
                Size = uf.Size,
                Ext = uf.Ext,

            }).ToList();
            _logger.LogInformation($"{nameof(GetAllUploadFileAsync)} trigger function returned a response {JsonConvert.SerializeObject(SupplierLists)}");
            return SupplierLists;
        }

        public async Task<UploadFileDto> GetUploadFileByIdAsync(int id)
        {
            _logger.LogInformation($"{nameof(GetUploadFileByIdAsync)} trigger function received a request for upload file with Id: {id}");
            //string serverPath = _config.GetValue<string>("UploadDir:ImageUploadDirectory");
            var upload = await _uploadFileRepository.Queryable
                .OrderByDescending(uf => uf.Id)
                .FirstOrDefaultAsync(b => b.Id == id);

            var uploadFile = new UploadFileDto
            {
                Id = upload.Id,
                Name = upload.Name,
                AlternativeText = upload.AlternativeText,
                Hash = upload.Hash,
                Size = upload.Size,
                Mime = upload.Mime,
                Ext = upload.Ext,
                Url = upload.Url,
            };



            _logger.LogInformation($"{nameof(GetUploadFileByIdAsync)} trigger function returned a response {JsonConvert.SerializeObject(uploadFile)}");

            return uploadFile;
        }

        public async Task<AddUploadFileDto> AddUploadFile(AddUploadFileDto request)
        {

            if (request.Images.Length > 2 * 1024 * 1024)
            {
                throw new Exception("The selected image size exceeds 2 MB.");
            }

            var extension = Path.GetExtension(request.Images.FileName);
            if (!extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                !extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Image with extension JPG, JPEG and PNG are only supported.");
            }


            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var randomFileName = Path.GetRandomFileName().Replace(".", "");

            var fileName = randomFileName + extension;

            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Images.CopyToAsync(stream);
            }

            // Construct the URL relative to the web root
            string relativeUrl = Path.Combine("/uploads", fileName);

            UploadFile image = new UploadFile
            {
                Name = request.Name,
                AlternativeText = request.AlternativeText,
                Hash = fileName,
                Ext = extension,
                Mime = request.Images.ContentType,
                Size = request.Images.Length,
                Url = relativeUrl,
            };

            await _uploadFileRepository.AddAsync(image);
            await _uploadFileRepository.SaveChangesAsync();

            return request;


        }
        public async Task<int> UpdateUploadFile(AddUploadFileDto request)
        {
            _logger.LogInformation($"{nameof(UpdateUploadFile)} trigger function received a request to update an image");

            var existingImage = await _uploadFileRepository.Queryable.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (request.Images != null && request.Images.Length > 0)
            {
                // Specify the server path where you want to save the file
                string serverPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads");

                //string serverPath = _config.GetValue<string>("UploadDir:ImageUploadDirectory");

                // Ensure the directory exists, create it if not
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                // Create a unique file name 
                string imageFileName = Guid.NewGuid().ToString();

                // Combine the server path with the file name
                string fileName = imageFileName + Path.GetExtension(request.Images.FileName);

                string filePath = Path.Combine(serverPath, fileName);

                // Open a stream to write the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Images.CopyToAsync(stream);
                }

                existingImage.Name = request.Name;
                existingImage.AlternativeText = request.AlternativeText;
                existingImage.Hash = imageFileName;
                existingImage.Ext = Path.GetExtension(request.Images.FileName);
                existingImage.Mime = request.Images.ContentType;
                existingImage.Size = request.Images.Length;
                existingImage.Url = "/uploads/" + fileName;
            }
            else
            {
                // Update image properties without changing the image file
                existingImage.Name = request.Name;
                existingImage.AlternativeText = request.AlternativeText;
                
            }

            await _uploadFileRepository.UpdateAsync(existingImage);
            await _uploadFileRepository.SaveChangesAsync();

            return request.Id;
        }

        public async Task<UploadFileDto> DeleteUploadFile(int id)
        {
            _logger.LogInformation($"{nameof(DeleteUploadFile)} trigger function received a request to delete an image");

            var images = await _uploadFileRepository.Queryable.FirstOrDefaultAsync(b => b.Id == id);

            var img = new UploadFileDto { Id = id };

            // Specify the server path where the image file is located
            string serverPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads");
            //string serverPath = _config.GetValue<string>("UploadDir:ImageUploadDirectory");

            // Delete the image file from the server
            var imagePath = Path.Combine(serverPath, images.Hash);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            await _uploadFileRepository.RemoveAsync(images);
            await _uploadFileRepository.SaveChangesAsync();

            return img;
        }



    }
}
