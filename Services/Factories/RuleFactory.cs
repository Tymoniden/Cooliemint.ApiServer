using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class RuleFactory
    {
        public RuleDto CreateRule(RuleModel rule)
        {
            return new RuleDto(rule.Id, rule.Name, rule.Description, rule.IsExecuted, rule.LastExecuted);
        }

        public RuleModel CreateRuleModel(RuleDto ruleDto)
        {
            return new RuleModel
            {
                Id = ruleDto.Id,
                Name = ruleDto.Name,
                Description = ruleDto.Description,
                IsExecuted = ruleDto.IsExecuted,
                LastExecuted = ruleDto.LastExecuted
            };
        }
    }
}
