namespace Cooliemint.ApiServer.Models
{
    public class Rule
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<RulePart> Parts { get; set; } = [];

        public List<RuleCommand> Commands { get; set; } = [];

        public List<ResetCondition> ResetConditions { get; set; } = [];

        public List<RuleNotification> Notifications { get; set; } = [];

        public bool IsExecuted { get; set; }

        public DateTime? LastExecuted { get; set; }
    }
}
