using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Messaging;

namespace Cooliemint.ApiServer.Services.Automation
{
    public class RulePartValueStoreValueStrategy(ValueStore valueStore) : IRulePartStrategy
    {
        public bool Validate(string operandLeft, string operandRight, RulePartOperation operation)
        {
            // TODO Think about other type declaration in rule part

            var value = valueStore.GetValue<string>(operandLeft);
            var compareValue = operandRight;
            
            if(operation == RulePartOperation.Equals)
            {
                return value?.Equals(compareValue) ?? false;
            }

            var numericOperandLeft = decimal.Parse(value ?? "0");
            var numericOperandRight = decimal.Parse(operandRight);

            switch (operation)
            {
                case RulePartOperation.LessThan:
                    return numericOperandLeft < numericOperandRight;
                case RulePartOperation.LessThanOrEquals:
                    return numericOperandLeft <= numericOperandRight;
                case RulePartOperation.GreaterThan:
                    return numericOperandLeft > numericOperandRight;
                case RulePartOperation.GreaterThanOrEquals:
                    return numericOperandLeft >= numericOperandRight;
            }

            return false;
        }
    }
}
