using API;
using Application.Dto.Student;
using Application.Features.Student.Command;
using Application.Features.Student.Query;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortfolio_Management_System.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseApiController
    {
        private readonly IValidator<AddStudentDto> _validator;

        public StudentController(IValidator<AddStudentDto> validator)
        {
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IQueryable<StudentDto>>> GetAllStudents([FromQuery] string faculty = null, [FromQuery] string semester = null)
        {
            var students = await Mediator.Send(new GetAllStudentsQuery { Faculty = faculty, Semester = semester });
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentDto studentDto)
        {
            // Validate the studentDto
            var validationResult = await _validator.ValidateAsync(studentDto);
            if (!validationResult.IsValid)
            {
                // Return BadRequest if validation fails
                return BadRequest(validationResult.Errors);
            }

            // If validation passes, proceed with adding the student
            var studentId = await Mediator.Send(new AddStudentCommand { Student = studentDto });
            return Ok(studentId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            var studentDto = await Mediator.Send(new GetStudentByIdQuery { Id = id });
            return studentDto != null ? Ok(studentDto) : NotFound();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromForm] UpdateStudentCommand studentDto)
        {
            var result = await Mediator.Send(studentDto);
            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await Mediator.Send(new DeleteStudentCommand { Id = id });
            return result ? Ok(result) : NotFound();
        }
    }
}
