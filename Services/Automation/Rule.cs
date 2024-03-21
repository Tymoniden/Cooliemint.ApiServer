namespace Cooliemint.ApiServer.Services.Automation;

public class Rule
{
    public List<IAutomationAction> Actions { get; set; } = new();
    public List<ICondition> Conditions { get; set; } = new();
}