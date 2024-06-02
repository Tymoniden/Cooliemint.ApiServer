namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverMessageFactory
    {
        public PushoverMessageDto CreateMessage(AppNotification appNotification)
        {
            var message = new PushoverMessageDto
            {
                Title = appNotification.Title,
                Message = appNotification.Message
            };

            if (appNotification.Uri != null)
            {
                message.Url = appNotification.Uri.ToString();
                message.UrlTitle = appNotification.UriName;
            }

            return message;
        }

        public PushoverMessageDto CreateMessage(string title, string message)
        {
            return CreateMessage(title, message, null, null);
        }

        public PushoverMessageDto CreateMessage(string title, string message, string? url , string? urlTitle)
        {
            var messageDto = new PushoverMessageDto
            {
                Title= title,
                Message = message
            };

            if(url != null)
            {
                messageDto.Url = url;
            }

            if(urlTitle != null)
            {
                messageDto.UrlTitle = urlTitle;
            }

            return messageDto;
        }

        public Dictionary<string, string> CreateDictionary(PushoverMessageDto message, string token, string user)
        {
            return new Dictionary<string, string>
            {
                ["token"] = token,
                ["user"] = user,
                ["message"] = message.Message,
                ["title"] = message.Title
            };
        }
    }
}
