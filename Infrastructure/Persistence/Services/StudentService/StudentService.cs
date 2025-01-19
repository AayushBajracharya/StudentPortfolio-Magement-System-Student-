using Application.Dto.Student;
using Application.Interfaces.Repositories.StudentRepository;
using Application.Interfaces.Services.StudentService;
using Microsoft.AspNetCore.Hosting;

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

        public async Task<IQueryable<StudentDto>> GetAllStudentAsync(string faculty = null, string semester = null, int pageNumber = 1, int pageSize = 5)
        {
            var studentsQuery = _studentRepository.GetQueryable();

            // Apply filters if provided
            if (!string.IsNullOrEmpty(faculty))
            {
                studentsQuery = studentsQuery.Where(s => s.Faculty == faculty);
            }

            if (!string.IsNullOrEmpty(semester))
            {
                studentsQuery = studentsQuery.Where(s => s.Semester == semester);
            }

            // Apply pagination
            studentsQuery = studentsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

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

            return studentDtos;
        }


        public async Task<int> AddStudentAsync(AddStudentDto studentDto)
        {

            // List to hold the image URLs after validation and saving
            var imageUrls = new List<string>();

            if (studentDto.Images != null && studentDto.Images.Any())
            {
                foreach (var image in studentDto.Images)
                {
                    // Image validation
                    if (image.Length > 2 * 1024 * 1024)  // Check if image size exceeds 2 MB
                    {
                        throw new Exception("Image size exceeds 2 MB.");
                    }

                    var extension = Path.GetExtension(image.FileName);  // Get file extension
                    if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension.ToLower()))  // Check if format is allowed
                    {
                        throw new Exception("Supported formats are .jpg, .jpeg, and .png.");
                    }

                    // Save the image in the "uploads" directory
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid() + extension;  // Create a unique file name
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);  // Save the image
                    }

                    // Add the image URL (relative to the web root) to the list
                    imageUrls.Add(Path.Combine("uploads", fileName));
                }
            }

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
            student.ImageUrl = studentDto.ImageUrl;

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
    }
}
