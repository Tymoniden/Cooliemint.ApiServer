using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Messaging;

namespace Cooliemint.ApiServer.Services.Automation
{
    public class RulePartValueStoreValueAgeStrategy(ValueStore valueStore) : IRulePartStrategy
    {
        public bool Validate(string operandLeft, string operandRight, RulePartOperation operation)
        {
            var timeSpanCreated = valueStore.GetCreationDate(operandLeft);
            var config = TimeSpan.Parse(operandRight);
            var difference = DateTime.Now.Subtract(config);

            switch (operation)
            {
                case RulePartOperation.Equals:
                    return difference == timeSpanCreated;
                case RulePartOperation.LessThan:
                    return difference < timeSpanCreated;
                case RulePartOperation.LessThanOrEquals:
                    return difference <= timeSpanCreated;
                case RulePartOperation.GreaterThan:
                    return difference > timeSpanCreated;
                case RulePartOperation.GreaterThanOrEquals:
                    return difference >= timeSpanCreated;
            }

            return false;
        }
    }
}
