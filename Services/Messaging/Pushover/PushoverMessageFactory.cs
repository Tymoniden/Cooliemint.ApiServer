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
