using Application.Dto.Patient;
using Application.Features.Patient.Command;
using Application.Wrapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [Route("patient")]
    [ApiController]
    public class PatientController : BaseApiController
    {
        private readonly IValidator<PatientDto> _validator;

        public PatientController(IValidator<PatientDto> validator)
        {
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PatientRegisterCommand command)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command);
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

    }
}
