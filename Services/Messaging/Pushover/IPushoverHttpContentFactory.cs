using Cooliemint.ApiServer.Services.Messaging;
using Cooliemint.ApiServer.Services.Messaging.Pushover;

namespace CoolieMint.WebApp.Services.Notification.Pushover
{
    public interface IPushoverHttpContentFactory
    {
        PushoverHttpContent CreateHttpContent(AppNotification appNotification, string applicationKey, string userKey);
    }
}