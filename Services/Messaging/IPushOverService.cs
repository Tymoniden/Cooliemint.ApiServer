namespace Cooliemint.ApiServer.Services.Messaging
{
    public interface IPushOverService
    {
        Task SendMessage(AppNotification appNotification);
    }
}
