using Cooliemint.ApiServer.Services.Messaging.Pushover;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public interface IPushOverService
    {
        Task SendMessage(AppNotification appNotification, CancellationToken cancellationToken);
        Task SendMessage(PushoverMessageDto message, CancellationToken cancellationToken);
        Task SendMessageToAccount(PushoverMessageDto message, List<PushoverAccountDto> pushoverAccounts, CancellationToken cancellationToken);
    }
}
