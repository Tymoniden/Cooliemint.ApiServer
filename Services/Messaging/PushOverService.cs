using Cooliemint.ApiServer.Services.Messaging.Pushover;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class PushOverService : IPushOverService
    {
        private readonly PushoverHttpClient _pushoverHttpClient;
        private readonly PushoverAccountStore _pushoverAccountStore;
        private readonly PushoverMessageFactory _messageFactory;

        public PushOverService(PushoverHttpClient pushoverHttpClient, PushoverAccountStore pushoverAccountStore, PushoverMessageFactory messageFactory)
        {
            _pushoverHttpClient = pushoverHttpClient ?? throw new ArgumentNullException(nameof(pushoverHttpClient));
            _pushoverAccountStore = pushoverAccountStore ?? throw new ArgumentNullException(nameof(pushoverAccountStore));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        public async Task SendMessage(AppNotification appNotification, CancellationToken cancellationToken)
        {
            var message = _messageFactory.CreateMessage(appNotification);
            foreach(var account in _pushoverAccountStore.GetAll())
            {
                await _pushoverHttpClient.Send(message, account.ApplicationKey, account.UserKey, cancellationToken);
            }
        }

        public async Task SendMessage(PushoverMessageDto message, CancellationToken cancellationToken)
        {
            foreach(var account in _pushoverAccountStore.GetAll())
            {
                await _pushoverHttpClient.Send(message, account.ApplicationKey, account.UserKey, cancellationToken);
            }
        }
    }
}
