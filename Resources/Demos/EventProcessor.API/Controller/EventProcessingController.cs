using System;
using System.Threading.Tasks;
using EventProcessor.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventProcessor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventProcessingController : ControllerBase
    {
        private readonly EventProcessingService _eventProcessingService;
        private readonly ILogger<EventProcessingController> _logger;

        public EventProcessingController(
            EventProcessingService eventProcessingService,
            ILogger<EventProcessingController> logger)
        {
            _eventProcessingService = eventProcessingService;
            _logger = logger;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessEventFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            try
            {
                _logger.LogInformation("Received event file: {FileName}", file.FileName);

                using var stream = file.OpenReadStream();
                await _eventProcessingService.ProcessEventFileAsync(stream);

                return Ok(new { message = "File processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing file: {FileName}", file.FileName);
                return StatusCode(500, "An error occurred while processing the file");
            }
        }
    }
}
