using Application.Dto.Teacher;

namespace Application.Interfaces.Services.TeacherService
{
    public interface ITeacherService
    {
        Task<AddTeacherDto> RegisterTeacher(AddTeacherDto teacherDto);
        Task<TeacherDto> GetTeacherDetails(int teacherId);
    }
}
