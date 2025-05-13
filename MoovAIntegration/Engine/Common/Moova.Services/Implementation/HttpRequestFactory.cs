using System.Diagnostics;
using System.Net.Http.Headers;
using Moova.Services.Interfaces;

namespace Moova.Services.Implementation;

public class HttpRequestFactory : IHttpRequestFactory
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    static HttpRequestFactory()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<(HttpResponseMessage response, TimeSpan requestTime)> SendAsync(HttpRequestMessage request,
        string token)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.SendAsync(request);
            stopwatch.Stop();
            var requestTime = stopwatch.Elapsed;
            return (response, requestTime);
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw;
        }
    }

    public void SetDefaultHeader(string name, string value)
    {
        if (Client.DefaultRequestHeaders.Contains(name)) Client.DefaultRequestHeaders.Remove(name);
        Client.DefaultRequestHeaders.Add(name, value);
    }

    private void LogError(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}