namespace Cooliemint.ApiServer.Services.Automation
{
    public class CheckValueCondition : ICondition
    {
        public ConditionType ConditionType => ConditionType.CheckValue;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
