namespace Cooliemint.ApiServer.Services
{
    public class GuidService : IGuidService
    {
        public Guid CreateNewGuid() => Guid.NewGuid();
    }
}
