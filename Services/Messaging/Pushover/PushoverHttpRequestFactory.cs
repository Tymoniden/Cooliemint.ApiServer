using CoolieMint.WebApp.Services.Notification.Pushover;
using System.Diagnostics;
using System.Text;

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverHttpRequestFactory : IPushoverHttpRequestFactory
    {
        readonly Uri _serverUri = new Uri("https://api.pushover.net/1/messages.json");
        private readonly JsonSerializerService _jsonSerializerService;
        private readonly IPushoverHttpContentFactory _pushoverHttpContentFactory;

        public PushoverHttpRequestFactory(JsonSerializerService jsonSerializerService, IPushoverHttpContentFactory pushoverHttpContentFactory)
        {
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
            _pushoverHttpContentFactory = pushoverHttpContentFactory ?? throw new ArgumentNullException(nameof(pushoverHttpContentFactory));
        }

        public HttpRequestMessage CreateHttpRequest(AppNotification appNotification, string userKey, string applicationKey)
        {
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, _serverUri);

                var pushoverHttpContent = _pushoverHttpContentFactory.CreateHttpContent(appNotification, applicationKey, userKey);
                var jsonContent = _jsonSerializerService.Serialize(pushoverHttpContent, SerializerSettings.ApiSerializer);
                message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                //_httpContentFactory.CreateJsonStringContent(jsonContent);

                return message;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }


}
