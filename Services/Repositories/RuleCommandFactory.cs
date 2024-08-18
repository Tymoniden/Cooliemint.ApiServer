using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Repositories
{
    public class RuleCommandFactory
    {
        public RuleCommandDto CreateRuleCommand(RuleCommandModel ruleCommandModel)
        {
            return new RuleCommandDto(ruleCommandModel.Id, ruleCommandModel.Name, ruleCommandModel.CommandType, ruleCommandModel.Value);
        }
    }
}
