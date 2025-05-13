using EventProcessor.Core.Entities;
using EventProcessor.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventProcessor.Application.Services
{
    public class EventDispatcherService : IEventDispatcher
    {
        private readonly ISystemYApiClient _apiClient;
        private readonly ILogger<EventDispatcherService> _logger;

        public EventDispatcherService(
            ISystemYApiClient apiClient,
            ILogger<EventDispatcherService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task DispatchAsync(object eventData)
        {
            try
            {
                _logger.LogInformation("Dispatching event of type {EventType}", eventData.GetType().Name);

                switch (eventData)
                {
                    case AccountingOperation accounting:
                        await _apiClient.SendAccountingOperationAsync(accounting);
                        break;

                    case Personalization personalization:
                        await _apiClient.SendPersonalizationEventAsync(personalization);
                        break;

                    case UnitReconstruction reconstruction:
                        await _apiClient.SendUnitReconstructionEventAsync(reconstruction);
                        break;

                    default:
                        _logger.LogWarning("Unsupported event type: {EventType}", eventData.GetType().Name);
                        throw new NotSupportedException($"Event type {eventData.GetType().Name} is not supported");
                }

                _logger.LogInformation("Successfully dispatched event of type {EventType}", eventData.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dispatch event of type {EventType}", eventData.GetType().Name);
                throw;
            }
        }
    }
}
