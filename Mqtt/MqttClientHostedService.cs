using MQTTnet.Client;
using MQTTnet;
using System.Diagnostics;
using Cooliemint.ApiServer.Services;

namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MqttClientHostedService : IHostedService
    {
        private readonly IMessageConverterService _messageConverterService;
        private readonly IMessageStore _messageStore;
        private readonly ConfigurationService _configurationService;
        private MqttClientOptions? _clientOptions;
        private IMqttClient? _client;
        private MqttFactory _mqttFactory = new MqttFactory();

        public MqttClientHostedService(IMessageConverterService messageConverterService, IMessageStore messageStore, ConfigurationService configurationService)
        {
            _messageConverterService = messageConverterService ?? throw new ArgumentNullException(nameof(messageConverterService));
            _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var configuration = _configurationService.GetConfiguration();

            if(!string.IsNullOrEmpty(configuration.MqttServer) && configuration.MqttPort > 0)
            {
                Debug.WriteLine("Configuration set at startup");
                await ConnectToBroker(configuration.MqttServer!, configuration.MqttPort, cancellationToken);
            }
            else
            {
                Debug.WriteLine("Registering configuration changed event");
                _configurationService.ConfigurationChanged += (_, args) =>
                {
                    Debug.WriteLine("Configuration was changed");
                    if (!string.IsNullOrEmpty(args.Configuration.MqttServer) && args.Configuration.MqttPort > 0)
                    {
                        Task.Run(async () =>
                        {
                            await ConnectToBroker(args.Configuration.MqttServer!, args.Configuration.MqttPort, cancellationToken);
                        }, cancellationToken);
                    }   
                };
            }
        }

        async Task ConnectToBroker(string broker, int port, CancellationToken cancellationToken)
        {
            Debug.WriteLine("connecting to broker");
            _clientOptions = new MqttClientOptionsBuilder().WithTcpServer(broker, port).Build();
            _client = _mqttFactory.CreateMqttClient();
            _client.ApplicationMessageReceivedAsync += e =>
            {
                _messageStore.AddMessage(_messageConverterService.CreateMqttMessage(e));

                return Task.CompletedTask;
            };

            await _client.ConnectAsync(_clientOptions, cancellationToken);

            var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic("shellies/#"))
                .Build();

            await _client.SubscribeAsync(mqttSubscribeOptions, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_client == null) return;

            var disconnectOptions = _mqttFactory.CreateClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build();
            await _client.DisconnectAsync(disconnectOptions, cancellationToken);
            _client.Dispose();
        }
    }
}
