namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public class PushoverHttpContent
    {
        public string user { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string url_title { get; set; } = string.Empty;
        public int priority { get; set; }
        public string timestamp { get; set; } = string.Empty;
        public string sound { get; set; } = string.Empty;
        public string attachment { get; set; } = string.Empty;
    }
}
