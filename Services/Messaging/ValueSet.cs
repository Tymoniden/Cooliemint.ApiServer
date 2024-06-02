namespace Cooliemint.ApiServer.Services.Messaging
{
    public record ValueSet(string Key, object Value, DateTime CreationTime, DateTime LastSeen);
}
