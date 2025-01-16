using Application.Dto.Student;
using Domain.Entities;

namespace Application.Interfaces.Services.StudentService
{
    public interface IStudentService
    {
        Task<IQueryable<StudentDto>> GetAllStudentAsync(string faculty = null, string semester = null);
        Task<StudentDto> GetStudentByIdAsync(int id);
        Task<int> AddStudentAsync(AddStudentDto studentDto);
        Task<bool> UpdateStudentAsync(UpdateStudentDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
