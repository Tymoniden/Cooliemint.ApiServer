namespace Cooliemint.ApiServer.Services.Automation;

public class SetValueAction : IAutomationAction
{
    public ActionType ActionType => ActionType.SetValue;
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}