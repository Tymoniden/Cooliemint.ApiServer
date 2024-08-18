namespace Cooliemint.ApiServer.Models
{
    public record RuleCommandModel(string Name, RuleCommandType CommandType, string Value, int Id = 0);
}
