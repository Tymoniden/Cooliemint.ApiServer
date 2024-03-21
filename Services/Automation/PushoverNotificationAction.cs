namespace Cooliemint.ApiServer.Services.Automation;

public class PushoverNotificationAction : IAutomationAction
{
    public ActionType ActionType => ActionType.PushoverNotification;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}