using Cooliemint.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cooliemint.ApiServer.SmartDeviceDiscovery
{
    [Route("api/SmartDevice")]
    [ApiController]
    public class SmartDeviceController(SmartDeviceDiscoveryProvider smartDeviceDiscoveryProvider, SmartDeviceFactory smartDeviceFactory) : ControllerBase
    {
        [HttpGet()]
        public List<SmartDeviceDto> Get()
        {
            return smartDeviceDiscoveryProvider
                .GetDevices()
                .Select(smartDeviceFactory.CreateSmartDeviceDto)
                .ToList();
        }
    }
}
