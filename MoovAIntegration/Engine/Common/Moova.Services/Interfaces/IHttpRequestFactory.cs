namespace Moova.Services.Interfaces;

public interface IHttpRequestFactory
{
    Task<(HttpResponseMessage response, TimeSpan requestTime)> SendAsync(HttpRequestMessage request, string token);
    void SetDefaultHeader(string name, string value);
}