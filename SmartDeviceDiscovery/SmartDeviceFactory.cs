using Cooliemint.Shared.Dtos;

namespace Cooliemint.ApiServer.SmartDeviceDiscovery
{
    public class SmartDeviceFactory
    {
        public SmartDeviceDto CreateSmartDeviceDto(SmartDeviceModel model)
        {
            return new SmartDeviceDto(model.Id, model.IsDiscovered, model.IsAdded);
        }
    }
}
