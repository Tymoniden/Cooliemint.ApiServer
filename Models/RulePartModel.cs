using System.ComponentModel.DataAnnotations.Schema;

namespace Cooliemint.ApiServer.Models
{
    public class RulePartModel
    {
        public int Id { get; set; }

        public RulePartType Type { get; set; }

        public RulePartDescriptor Descriptor { get; set; }

        public required string Description { get; set; }

        public required string OperandLeft { get; set; }

        public required string OperandRight { get; set; }

        public required RulePartOperation Operation { get; set; }
    }
}
