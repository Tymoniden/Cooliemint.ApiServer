namespace Cooliemint.ApiServer.Services.Automation;

public class MqttTopicCondition : ICondition
{
    public ConditionType ConditionType => ConditionType.MqttTopic;
    public string Topic { get; set; } = string.Empty;
}