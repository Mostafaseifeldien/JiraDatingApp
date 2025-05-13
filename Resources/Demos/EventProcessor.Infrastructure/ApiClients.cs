using System.Net.Http.Json;
using EventProcessor.Core.Entities;
using EventProcessor.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventProcessor.Infrastructure.ApiClients
{
    public class SystemYApiClient : ISystemYApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SystemYApiClient> _logger;

        public SystemYApiClient(HttpClient httpClient, ILogger<SystemYApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendAccountingOperationAsync(AccountingOperation operation)
        {
            await PostEventAsync("api/events/accounting", operation, operation.CardId.ToString());
        }

        public async Task SendPersonalizationEventAsync(Personalization personalization)
        {
            await PostEventAsync("api/events/personalization", personalization, personalization.Header.SerialNumber.ToString());
        }

        public async Task SendUnitReconstructionEventAsync(UnitReconstruction reconstruction)
        {
            await PostEventAsync("api/events/reconstruction/unit", reconstruction, reconstruction.Header.SerialNumber.ToString());
        }

        private async Task PostEventAsync(string url, object payload, string identifier)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send event to {Url}. Status: {StatusCode}, Response: {Response}", url, response.StatusCode, content);
                    throw new HttpRequestException($"API request failed with status {response.StatusCode}");
                }

                _logger.LogInformation("Successfully sent event to {Url} for ID {Identifier}", url, identifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending event to System Y: {Url}", url);
                throw;
            }
        }
    }
}
