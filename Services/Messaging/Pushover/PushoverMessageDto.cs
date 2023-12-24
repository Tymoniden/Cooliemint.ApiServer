namespace Cooliemint.ApiServer.Services.Messaging.Pushover;

public class PushoverMessageDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string UrlTitle { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    // TODO make this an enum
    public string Sound { get; set; } = string.Empty;
    public string Attachment { get; set; } = string.Empty;
}