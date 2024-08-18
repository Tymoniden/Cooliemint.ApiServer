using Cooliemint.ApiServer.Models;


namespace Cooliemint.ApiServer.Services.Automation
{
    public class RuleValidationHandler(RulePartValidator rulePartValidator, RulePartValidationCache rulePartValidationCache)
    {
        public bool ExecuteConditionsMatch(RuleModel rule)
        {
            var ruleWasSuccessful = true;

            foreach (var rulePart in rule.Parts.Where(p => p.Type == RulePartType.ExecuteCondition))
            {
                var result = rulePartValidator.Validate(rulePart);
                if (!result)
                {
                    ruleWasSuccessful = false;
                }

                rulePartValidationCache.SetValue(rulePart.Id, result);
            }

            return ruleWasSuccessful;
        }

        public bool ResetConditionsMatch(RuleModel rule)
        {
            foreach (var rulePart in rule.Parts.Where(part => part.Type == RulePartType.ResetCondition))
            {
                if (rulePartValidator.Validate(rulePart))
                {
                    rulePartValidationCache.SetValue(rulePart.Id, true);
                    return true;
                }
            }

            return false;
        }
    }
}
