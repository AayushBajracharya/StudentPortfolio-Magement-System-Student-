using Application.Features.Doctor.Command.AddDoctorCommand;
using Application.Features.Doctor.Query.DoctorList;
using Application.Features.Doctor.Query.SearchDoctor;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [Route("doctor")]
    [ApiController]
    public class DoctorController : BaseApiController
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetDoctorsList([FromQuery] DoctorListQuery query)
        {
            try
            {
                var response = await Mediator.Send(query);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }

        [HttpPost("register")]

        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorCommand command)
        {
            try
            {

                var doctor = await Mediator.Send(command);

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDoctors(string searchTerm)
        {
            try
            {
                var response = await Mediator.Send(new SearchDoctorQuery { searchTerm = searchTerm });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }



    }
}
