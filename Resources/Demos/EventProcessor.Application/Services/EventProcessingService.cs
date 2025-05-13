using EventProcessor.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventProcessor.Application.Services
{
    public class EventProcessingService
    {
        private readonly IFileParser _fileParser;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger<EventProcessingService> _logger;

        public EventProcessingService(
            IFileParser fileParser,
            IEventDispatcher eventDispatcher,
            ILogger<EventProcessingService> logger)
        {
            _fileParser = fileParser;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
        }

        public async Task ProcessEventFileAsync(Stream fileStream)
        {
            try
            {
                _logger.LogInformation("Starting event file processing");

                var events = await _fileParser.ParseEventFileAsync(fileStream);
                _logger.LogInformation("Parsed {EventCount} events from file", events.Count());

                foreach (var eventData in events)
                {
                    await _eventDispatcher.DispatchAsync(eventData);
                }

                _logger.LogInformation("Completed processing event file");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event file");
                throw;
            }
        }
    }
}
