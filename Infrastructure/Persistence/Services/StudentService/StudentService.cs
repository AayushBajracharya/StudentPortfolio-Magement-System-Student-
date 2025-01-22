using System.Linq;
using Application.Dto.Student;
using Application.Interfaces.Repositories.StudentRepository;
using Application.Interfaces.Services.StudentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StudentService(IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<(IQueryable<StudentDto> students, int totalCount)> GetAllStudentAsync(
         string faculty = null,
         string semester = null,
         string name = null,
         int pageNumber = 1,
         int pageSize = 0)
        {
            var studentsQuery = _studentRepository.GetQueryable();

            // Apply filters if provided
            studentsQuery = studentsQuery
                .Where(s => (string.IsNullOrEmpty(faculty) || s.Faculty == faculty) &&
                            (string.IsNullOrEmpty(semester) || s.Semester == semester) &&
                            (string.IsNullOrEmpty(name) || s.Name.Contains(name)));


            // Get the total count before pagination
            var totalCount = studentsQuery.Count();

            // Apply pagination if pageSize is greater than 0
            if (pageSize > 0)
            {
                studentsQuery = studentsQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            // Map to StudentDto
            var studentDtos = studentsQuery.Select(student => new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                FatherName = student.FatherName,
                MotherName = student.MotherName,
                Gender = student.Gender,
                DoB = student.DoB,
                Email = student.Email,
                Faculty = student.Faculty,
                Semester = student.Semester,
                PhoneNo = student.PhoneNo,
                ImageUrl = student.ImageUrl
            });

            return (studentDtos, totalCount);
        }



        public async Task<int> AddStudentAsync(AddStudentDto studentDto)
        {
            var imageUrls = await SaveStudentImagesAsync(studentDto.Images);

            var student = new Student
            {
                Name = studentDto.Name,
                FatherName = studentDto.FatherName,
                MotherName = studentDto.MotherName,
                Gender = studentDto.Gender,
                DoB = studentDto.DoB,
                Email = studentDto.Email,
                Password = studentDto.Password,
                Faculty = studentDto.Faculty,
                Semester = studentDto.Semester,
                PhoneNo = studentDto.PhoneNo,
                ImageUrl = string.Join(";", imageUrls)
            };

            var addedStudent = await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();
            return addedStudent.Id;
        }


        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) return null;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                FatherName = student.FatherName,
                MotherName = student.MotherName,
                Gender = student.Gender,
                DoB = student.DoB,
                Email = student.Email,
                Faculty = student.Faculty,
                Semester = student.Semester,
                PhoneNo = student.PhoneNo,
                ImageUrl = student.ImageUrl
            };
        }
        public async Task<bool> UpdateStudentAsync(UpdateStudentDto studentDto)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(x => x.Id == studentDto.Id);
            if (student == null) return false;

            student.Name = studentDto.Name;
            student.FatherName = studentDto.FatherName;
            student.MotherName = studentDto.MotherName;
            student.Gender = studentDto.Gender;
            student.DoB = studentDto.DoB;
            student.Email = studentDto.Email;
            student.Password = studentDto.Password;
            student.Faculty = studentDto.Faculty;
            student.Semester = studentDto.Semester;
            student.PhoneNo = studentDto.PhoneNo;

            // Save new images if provided
            if (studentDto.Images != null && studentDto.Images.Any())
            {
                var imageUrls = await SaveStudentImagesAsync(studentDto.Images);
                student.ImageUrl = string.Join(";", imageUrls);
            }

            await _studentRepository.UpdateAsync(student);
            await _studentRepository.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null) return false;

            await _studentRepository.RemoveAsync(student);
            await _studentRepository.SaveChangesAsync();

            return true;
        }

        private async Task<List<string>> SaveStudentImagesAsync(IEnumerable<IFormFile> images)
        {
            var imageUrls = new List<string>();

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    // Validate image size and format
                    var extension = Path.GetExtension(image.FileName).ToLower();
                    if (image.Length > 2 * 1024 * 1024 || !new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                    {
                        throw new Exception("Invalid image file. Supported formats are .jpg, .jpeg, and .png, and the size must not exceed 2 MB.");
                    }

                    // Define the upload folder
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    // Save the file with a unique name
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Add the saved file's relative URL
                    imageUrls.Add(Path.Combine("uploads", fileName));
                }
            }

            return imageUrls;
        }

    }
}
