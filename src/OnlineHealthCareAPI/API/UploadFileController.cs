using Application.Features.UploadFile.Command.AddUploadFileCommand;
using Application.Features.UploadFile.Command.DeleteUploadFileCommand;
using Application.Features.UploadFile.Command.UpdateUploadFileCommand;
using Application.Features.UploadFile.Query.UploadFileById;
using Application.Features.UploadFile.Query.UploadFileList;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API
{
    [Route("")]
    [ApiController]
    public class UploadFileController : BaseApiController
    {
        private ILogger<UploadFileController> _logger;

        public UploadFileController(
            ILogger<UploadFileController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;

        }

        [HttpGet("ImgUploads")]
        public async Task<IActionResult> GetAllUploadFile([FromQuery] UploadFileListQuery query)
        {
            _logger.LogInformation($"{nameof(GetAllUploadFile)} trigger function received a request with param query: '{JsonConvert.SerializeObject(query)}'");
            var response = await Mediator.Send(query);
            _logger.LogInformation($"{nameof(GetAllUploadFile)} trigger function returned a response {JsonConvert.SerializeObject(response)}");

            return Ok(response);

        }

        [HttpGet("ImgUploads/{id}")]

        public async Task<IActionResult> GetUploadFileById(int id)
        {
            _logger.LogInformation($"{nameof(GetUploadFileById)} trigger function received a request for upload file with Id: {id}");
            var response = await Mediator.Send(new UploadFileByIdQuery { Id = id });
            _logger.LogInformation($"{nameof(GetUploadFileById)} trigger function returned a response {JsonConvert.SerializeObject(response)}");

            return Ok(response);

        }

        [HttpPost("ImgUploads")]

        public async Task<IActionResult> AddUploadFile([FromForm] AddUploadFileCommand command)
        {
            try
            {
                _logger.LogInformation($"{nameof(AddUploadFile)} trigger function received a request with param command: '{JsonConvert.SerializeObject(command)}'");
                var response = await Mediator.Send(command);
                _logger.LogInformation($"{nameof(AddUploadFile)} trigger function added a new image with Id:");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(AddUploadFile)}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }

        [HttpPut("ImgUploads")]

        public async Task<IActionResult> UpdateUploadFile([FromForm] UpdateUploadFileCommand command)
        {
            try
            {
                _logger.LogInformation($"{nameof(UpdateUploadFile)} trigger function received a request with param command: '{JsonConvert.SerializeObject(command)}'");
                var response = await Mediator.Send(command);
                _logger.LogInformation($"{nameof(UpdateUploadFile)} trigger function updated image with Id: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateUploadFile)}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("ImgUploads/{id}")]

        public async Task<IActionResult> DeleteUploadFile(int id)
        {
            try
            {
                _logger.LogInformation($"{nameof(DeleteUploadFile)} trigger function received a request to delete images with Id: {id}");
                var response = await Mediator.Send(new DeleteUploadFileCommand { Id = id });
                _logger.LogInformation($"{nameof(DeleteUploadFile)} trigger function deleted images with Id: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteUploadFile)}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
