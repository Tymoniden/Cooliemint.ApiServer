// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public sealed class PushoverAccountDto
    {
        public int Id { get; set; }
        public string ApplicationKey { get; set; } = string.Empty;
        public string UserKey { get; set; } = string.Empty;
    }
}
