using Application.Dto.Student;
using Domain.Entities;

namespace Application.Interfaces.Services.StudentService
{
    public interface IStudentService
    {
        Task<(IQueryable<StudentDto> students, int totalCount)> GetAllStudentAsync(
            string faculty = null,
            string semester = null,
            string name = null,
            int pageNumber = 1,
            int pageSize = 5);
        Task<StudentDto> GetStudentByIdAsync(int id);
        Task<int> AddStudentAsync(AddStudentDto studentDto);
        Task<bool> UpdateStudentAsync(UpdateStudentDto studentDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
