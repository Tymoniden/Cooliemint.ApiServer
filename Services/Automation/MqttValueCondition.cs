namespace Cooliemint.ApiServer.Services.Automation;

public class MqttValueCondition : ICondition
{
    public ConditionType ConditionType => ConditionType.MqttValue;
    public string Value { get; set; } = string.Empty;
}