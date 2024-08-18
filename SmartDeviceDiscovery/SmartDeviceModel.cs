namespace Cooliemint.ApiServer.SmartDeviceDiscovery
{
    public class SmartDeviceModel
    {
        public required string Id { get; set; }
        public bool IsDiscovered { get; set; }
        
        public bool IsAdded { get; set; }
    }
}
