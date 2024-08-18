namespace Cooliemint.ApiServer.SmartDeviceDiscovery
{
    public class SmartDeviceDiscoveryProvider
    {
        private List<SmartDeviceModel> _smartDevices = [];

        public List<SmartDeviceModel> GetDevices(bool hideAdded = false)
        {
            if (hideAdded)
                return _smartDevices.Where(device => !device.IsAdded).ToList();

            return _smartDevices;
        }

        public void RegisterSmartDevice(SmartDeviceModel smartDevice)
        {
            if (_smartDevices.Any(d => d.Id == smartDevice.Id))
                return;

            smartDevice.IsDiscovered = true;
            _smartDevices.Add(smartDevice);
        }
    }
}
