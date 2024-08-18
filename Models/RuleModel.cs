namespace Cooliemint.ApiServer.Models
{
    public class RuleModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<RulePartModel> Parts { get; set; } = [];

        public List<RuleCommandModel> Commands { get; set; } = [];

        public List<RuleNotification> Notifications { get; set; } = [];

        public bool IsExecuted { get; set; }

        public DateTime? LastExecuted { get; set; }
    }
}
