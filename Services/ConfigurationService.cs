using Cooliemint.ApiServer.Controllers;
using System.Diagnostics;

namespace Cooliemint.ApiServer.Services
{
    public class ConfigurationService
    {
        private ConfigurationDto _configuration = new ConfigurationDto();

        public event EventHandler<ConfigurationChangedEventArgs>? ConfigurationChanged;

        public void SetConfiguration(ConfigurationDto configuration)
        {
            _configuration = configuration;
            ConfigurationChanged?.Invoke(this, new ConfigurationChangedEventArgs(configuration));
            Debug.WriteLine("configuration updated");
        }

        public ConfigurationDto GetConfiguration()
        {
            return _configuration;
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
