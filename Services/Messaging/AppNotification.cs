using System;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class AppNotification
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Uri? Uri { get; set; }
        public string UriName { get; set; } = string.Empty;
    }
}
