using API;
using Application.Dto.Student;
using Application.Features.Student.Command;
using Application.Features.Student.Query;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortfolio_Management_System.API
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : BaseApiController
    {
        private readonly IValidator<AddStudentDto> _validator;

        public StudentController(IValidator<AddStudentDto> validator)
        {
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudents(
             [FromQuery] string faculty = null,
             [FromQuery] string semester = null,
             [FromQuery] string name = null,
             [FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 0)
        {
            var result = await Mediator.Send(new GetAllStudentsQuery
            {
                Faculty = faculty,
                Semester = semester,
                Name = name,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            var students = result.Item1;   // Extract the student list
            var totalCount = result.Item2; // Extract the total count

            return Ok(new
            {
                TotalCount = totalCount,
                Data = students
            });
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
