using Application.Dto.Doctor;
using Application.Dto.UploadFile;
using Application.Interfaces.Repositories.DoctorRepository;
using Application.Interfaces.Repositories.UploadFileMorphRepository;
using Application.Interfaces.Services.DoctorService;
using Application.Interfaces.Services.UploadFileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Services.DoctorService
{

    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUploadFileMorphRepository _uploadFileMorphRepository;

        public DoctorService(IDoctorRepository doctorRepository,
            IUploadFileMorphRepository uploadFileMorphRepository,
            IUploadFileService uploadFileService)
        {
            _doctorRepository = doctorRepository;
            _uploadFileMorphRepository = uploadFileMorphRepository;
            _uploadFileService = uploadFileService;
        }

        public async Task<AddDoctorDto> AddDoctorAsync(AddDoctorDto request)
        {
            var newDoctor = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Specialty = request.Specialty,
            };

            _doctorRepository.Add(newDoctor);
            await _doctorRepository.SaveChangesAsync();

            if (request.ImageIds != null && request.ImageIds.Any())
            {
                foreach (var imageId in request.ImageIds)
                {
                    var uploadFile = await _uploadFileService.GetUploadFileByIdAsync(imageId);

                    var uploadFileMorph = new UploadFileMorph
                    {
                        UploadFileId = imageId,
                        RelatedId = newDoctor.Id,
                        RelatedType = "doctors",
                        Field = "images",

                    };
                    _uploadFileMorphRepository.Add(uploadFileMorph);
                }
                await _uploadFileMorphRepository.SaveChangesAsync();
            }

            return request;

        }

        public async Task<List<DoctorDto>> GetAllDoctorAsync()
        {
            List<DoctorDto> doctorDtoList = new List<DoctorDto>();
            var doctorList = await _doctorRepository.Queryable.ToListAsync();

            foreach(var doctor in doctorList)
            {
                var doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Specialty = doctor.Specialty,
                    Images = new List<UploadFileDto>()
                };

                var imagesList = await _uploadFileMorphRepository.Queryable
                    .Where(x => x.RelatedId == doctor.Id && x.RelatedType == "doctors" && x.Field == "images")
                    .Include(m => m.UploadFile)
                    .ToListAsync();

                var imageDtoList = new List<UploadFileDto>();
                foreach (var img in imagesList)
                {
                    doctorDto.Images.Add(new UploadFileDto
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
                
                doctorDtoList.Add(doctorDto);
            }
            return doctorDtoList;
        }

        public async Task<List<DoctorDto>> SearchDoctorsAsync(string searchTerm)
        {
            var query =  _doctorRepository.Queryable;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(r =>
                    r.FirstName.ToLower().Contains(searchTerm) ||
                    r.LastName.ToLower().Contains(searchTerm) ||
                    r.Specialty.ToLower().Contains(searchTerm)
                );
            }

            var doctorList = await query.ToListAsync();

            List<DoctorDto> doctorDtoList = new List<DoctorDto>();

            foreach (var doctor in doctorList)
            {
                var doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Specialty = doctor.Specialty,
                    Images = new List<UploadFileDto>()
                };

                var imagesList = await _uploadFileMorphRepository.Queryable
                    .Where(x => x.RelatedId == doctor.Id && x.RelatedType == "doctors" && x.Field == "images")
                    .Include(m => m.UploadFile)
                    .ToListAsync();

                var imageDtoList = new List<UploadFileDto>();
                foreach (var img in imagesList)
                {
                    doctorDto.Images.Add(new UploadFileDto
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

                doctorDtoList.Add(doctorDto);
            }
            return doctorDtoList;
        }
    }
}
