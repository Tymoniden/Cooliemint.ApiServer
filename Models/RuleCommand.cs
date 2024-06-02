namespace Cooliemint.ApiServer.Models
{
    public record RuleCommand(string Name, RuleCommandType CommandType, string Value, int Id = 0);
}
