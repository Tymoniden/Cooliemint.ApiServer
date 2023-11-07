using Cooliemint.ApiServer.Services.Messaging.Pushover;
using CoolieMint.WebApp.Services.Notification.Pushover;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class PushOverService : IPushOverService
    {
        private readonly IPushoverHttpRequestFactory _pushoverHttpRequestFactory;
        private readonly PushoverHttpClient _pushoverHttpClient;
        private readonly PushoverAccountStore _pushoverAccountStore;

        public PushOverService(IPushoverHttpRequestFactory pushoverHttpRequestFactory, PushoverHttpClient pushoverHttpClient, PushoverAccountStore pushoverAccountStore)
        {
            _pushoverHttpRequestFactory = pushoverHttpRequestFactory ?? throw new ArgumentNullException(nameof(pushoverHttpRequestFactory));
            _pushoverHttpClient = pushoverHttpClient ?? throw new ArgumentNullException(nameof(pushoverHttpClient));
            _pushoverAccountStore = pushoverAccountStore ?? throw new ArgumentNullException(nameof(pushoverAccountStore));
        }

        public async Task SendMessage(AppNotification appNotification)
        {
            foreach(var account in _pushoverAccountStore.GetAll())
            {
                await _pushoverHttpClient.Send(_pushoverHttpRequestFactory.CreateHttpRequest(appNotification, account.ApplicationKey, account.UserKey));
            }
        }
    }
}
