using Cooliemint.ApiServer.Controllers;
using System.Diagnostics;

namespace Cooliemint.ApiServer.Services
{
    public class ConfigurationService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly JsonSerializerService _jsonSerializerService;
        private ConfigurationDto _configuration = new ConfigurationDto();
        private const string ConfigurationFile = "configuration.json";

        public event EventHandler<ConfigurationChangedEventArgs>? ConfigurationChanged;

        public ConfigurationService(IFileSystemService fileSystemService, JsonSerializerService jsonSerializerService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
        }

        public void SetConfiguration(ConfigurationDto configuration)
        {
            _configuration = configuration;
            ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs(configuration));
            _fileSystemService.WriteAllText(ConfigurationFile, _jsonSerializerService.Serialize(_configuration));
        }

        public ConfigurationDto GetConfiguration()
        {
            return _configuration;
        }

        public void Initialize()
        {
            try
            {
                var conf = _jsonSerializerService.Deserialize<ConfigurationDto>(_fileSystemService.ReadAllText(ConfigurationFile));
                if (conf != null)
                {
                    _configuration = conf;
                }
            }
            catch
            {
                // maybe track?
            }
        }
    }

    public sealed class ConfigurationChangedEventArgs : EventArgs
    {
        public ConfigurationChangedEventArgs(ConfigurationDto configuration)
        {
            Configuration = configuration;
        }

        public ConfigurationDto Configuration { get; set; }
    }
}
