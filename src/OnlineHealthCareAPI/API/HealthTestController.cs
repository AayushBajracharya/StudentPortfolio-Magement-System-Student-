using Application.Features.HealthTest.Command;
using Application.Features.HealthTest.Query;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [Route("")]
    [ApiController]
    public class HealthTestController : BaseApiController
    {
        //private readonly IMediator _mediator;
        private readonly ILogger<HealthTestController> _logger;

        public HealthTestController(ILogger<HealthTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GethealthTestList([FromQuery] HealthTestListQuery query)
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


        [HttpPost("Create")]
        public async Task<IActionResult> AddHealthTest([FromBody] AddHealthTestCommand command)
        {
            try
            {
                var healthtest = await Mediator.Send(command);
                return Ok(healthtest);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }



    }
}
