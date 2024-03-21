namespace Cooliemint.ApiServer.Services.Automation;

public class MqttMessageAction : IAutomationAction
{
    public ActionType ActionType => ActionType.MqttMessage;
    public string Topic { get; set; }
    public string Payload { get; set; }
}