using Microsoft.Net.Http.Headers;

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverHttpClient
    {
        private readonly HttpClient _httpClient;

        public PushoverHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.pushover.net/1/messages.json");

            httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Cooliemint.ApiServer");
        }

        public async Task<HttpResponseMessage> Send(HttpRequestMessage httpRequest)
        {
            return await _httpClient.SendAsync(httpRequest);
        }
    }
}
