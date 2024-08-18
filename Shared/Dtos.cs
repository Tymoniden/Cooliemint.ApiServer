using Cooliemint.ApiServer.Models;

namespace Cooliemint.ApiServer.Shared.Dtos
{
    public record NotificationDto(int Id, string Title, string Description);

    public record NotificationDetailsDto(int Id, string Title, string Description, string? LongDescription, string? Url, string? Icon);

    public record User(int Id, string Name, string? Email, UserNotification[] Notifications);

    public record UserNotification(int Id, int UserId, int NotificationId, bool IsActive);

    public record RuleDto(int Id, string Name, string Description, bool IsExecuted, DateTime? LastExecuted);

    public record RulePartDto(int Id, RulePartDescriptor Descriptor, string Description, string OperandLeft, string OperandRight, RulePartOperation Operation);

    public record RuleCommandDto(int Id, string Name, RuleCommandType CommandType, string Value);

    public record ContactProviderDto(int id, ContactProviderTypeDto Type, string Description, string Configuration);

    public enum ContactProviderTypeDto
    {
        Email = 0,
        Pushover = 1
    }
}
