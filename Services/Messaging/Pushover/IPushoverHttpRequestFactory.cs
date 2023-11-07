using Cooliemint.ApiServer.Services.Messaging;
using System.Net.Http;

namespace CoolieMint.WebApp.Services.Notification.Pushover
{
    public interface IPushoverHttpRequestFactory
    {
        HttpRequestMessage CreateHttpRequest(AppNotification appNotification, string userKey, string applicationKey);
    }
}