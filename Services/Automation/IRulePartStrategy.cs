using Cooliemint.ApiServer.Models;

namespace Cooliemint.ApiServer.Services.Automation
{
    public interface IRulePartStrategy
    {
        bool Validate(string operandLeft, string operandRight, RulePartOperation operation);
    }
}
