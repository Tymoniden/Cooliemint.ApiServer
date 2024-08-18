using System.Reflection;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemInformationController(IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        // GET: api/<SystemInformationController>
        [HttpGet]
        public SystemInformation Get()
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            
            return new(assembly.GetName().Version ?? new Version(1, 0, 0), webHostEnvironment.EnvironmentName);
        }
    }

    public sealed record SystemInformation(Version AssemblyVersion, string EnvironmentName);
}
