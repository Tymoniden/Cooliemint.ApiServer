using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Shared.Dtos;

namespace Cooliemint.ApiServer.Services.Factories
{
    public class RulePartFactory
    {
        public RulePartDto CreateRulePartDto(RulePartModel rulePart)
        {
            return new RulePartDto(
                rulePart.Id,
                rulePart.Descriptor,
                rulePart.Description,
                rulePart.OperandLeft,
                rulePart.OperandRight,
                rulePart.Operation
            );
        }

        public RulePartModel CreateRulePartModel(RulePartDto rulePartDto)
        {
            return new RulePartModel
            {
                Id = rulePartDto.Id,
                Descriptor = rulePartDto.Descriptor,
                Description = rulePartDto.Description,
                OperandLeft = rulePartDto.OperandLeft,
                OperandRight = rulePartDto.OperandRight,
                Operation = rulePartDto.Operation
            };
        }
    }
}
