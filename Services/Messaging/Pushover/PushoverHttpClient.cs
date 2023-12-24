using Microsoft.Net.Http.Headers;

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly PushoverMessageFactory _messageFactory;

        public PushoverHttpClient(HttpClient httpClient, PushoverMessageFactory messageFactory)
        {
            _httpClient = httpClient;
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
            _httpClient.BaseAddress = new Uri("https://api.pushover.net/1/messages.json");

            httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Cooliemint.ApiServer");
        }
        
        public async Task Send(PushoverMessageDto message, string token, string user, CancellationToken cancellationToken)
        {
            var parameters = _messageFactory.CreateDictionary(message, token, user);
            var content = new FormUrlEncodedContent(parameters);

            // if request uri is null or empty the base address will be used
            var response = await _httpClient.PostAsync(string.Empty, content, cancellationToken);
        }
    }
}
