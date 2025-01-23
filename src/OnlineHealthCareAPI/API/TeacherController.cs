using Application.Dto.Teacher;
using Application.Features.Teacher.Command;
using Application.Features.Teacher.Query;
using Application.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [Route("teacher")]
    [ApiController]
    public class TeacherController : BaseApiController
    {
        private readonly IValidator<AddTeacherDto> _validator;

        public TeacherController(IValidator<AddTeacherDto> validator)
        {
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] TeacherRegistrationCommand command)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command.AddTeacherDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var response = await Mediator.Send(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseWrapper(succeeded: false, message: $"Internal server error: {ex.Message}"));
            }
        }
        [HttpGet("GetTeacherById/{id}")]
        public async Task<IActionResult> GetTeacherById([FromForm] GetTeacherByIdQuery command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }


    }
}
