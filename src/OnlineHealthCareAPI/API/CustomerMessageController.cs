using Application.Dto.CustomerMessage;
using Application.Features.CustomerMessage.Command;
using Application.Interfaces.Services.CustomerMessageService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerMessageController : BaseApiController
    {
        private readonly ICustomerMessageService _customerMessageService;

        public CustomerMessageController(ICustomerMessageService customerMessageService)
        {
            _customerMessageService = customerMessageService;
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomerMessage([FromBody] AddCustomerMessageCommand message)
        {
            try
            {

                var customMessage = await Mediator.Send(message);

                return Ok(customMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }
    }

}
