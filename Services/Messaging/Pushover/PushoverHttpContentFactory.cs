using CoolieMint.WebApp.Services.Notification.Pushover;
using System;

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverHttpContentFactory : IPushoverHttpContentFactory
    {
        public PushoverHttpContent CreateHttpContent(AppNotification appNotification, string applicationKey, string userKey)
        {
            if(appNotification == null)
            {
                throw new ArgumentNullException(nameof(appNotification));
            }

            var content = new PushoverHttpContent
            {
                token = applicationKey,
                user = userKey,
                title = appNotification.Title,
                message = appNotification.Message
            };

            if(appNotification?.Uri != null)
            {
                content.url = appNotification.Uri.ToString();
                content.url_title = appNotification?.UriName ?? appNotification!.Uri.ToString();
            }

            return content;
        }
    }
}
