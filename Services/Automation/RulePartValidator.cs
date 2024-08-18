using Cooliemint.ApiServer.Models;

namespace Cooliemint.ApiServer.Services.Automation
{
    public class RulePartValidator
    {
        private readonly Dictionary<RulePartDescriptor, IRulePartStrategy> rulePartTypeStrategyMap = [];

        public RulePartValidator(RulePartValueStoreValueStrategy rulePartValueStoreValueStrategy,
            RulePartValueStoreValueAgeStrategy rulePartValueStoreValueAgeStrategy)
        {
            rulePartTypeStrategyMap.Add(RulePartDescriptor.ValueStore, rulePartValueStoreValueStrategy);
            rulePartTypeStrategyMap.Add(RulePartDescriptor.ValueStoreAge, rulePartValueStoreValueAgeStrategy);
        }

        public bool Validate(RulePartModel rulePart)
        {
            return rulePartTypeStrategyMap[rulePart.Descriptor].Validate(rulePart.OperandLeft, rulePart.OperandRight, rulePart.Operation);
        }
    }
}
