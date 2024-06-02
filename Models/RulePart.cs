using System.ComponentModel.DataAnnotations.Schema;

namespace Cooliemint.ApiServer.Models
{
    public class RulePart
    {
        public int Id { get; set; }

        public RulePartDescriptor Descriptor { get; set; }

        public required string Description { get; set; }

        public required string Value { get; set; }
    }

    public class ResetCondition
    {
        public int Id { get; set; }
        public Rule? Rule { get; set; }
        public RulePart? RulePart { get; set; }
    }
}
