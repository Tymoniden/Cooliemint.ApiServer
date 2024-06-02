using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationService _configurationService;
        private readonly CooliemintDbContext dbContext;

        public ConfigurationController(ConfigurationService configurationService, CooliemintDbContext dbContext)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // GET api/<ConfigurationController>/5
        [HttpGet]
        public ConfigurationDto Get()
        {
            return _configurationService.GetConfiguration();
        }

        // POST api/<ConfigurationController>
        [HttpPost]
        public void Post([FromBody] ConfigurationDto configuration)
        {
            _configurationService.SetConfiguration(configuration);
        }
    }

    public record ConfigurationDto
    {
        public string? MqttServer { get; set; }
        public int MqttPort { get; set; }

        public string? ConnectionString { get; set; }
    }
}
