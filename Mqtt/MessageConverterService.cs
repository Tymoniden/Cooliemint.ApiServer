using Cooliemint.ApiServer.Services;
using MQTTnet.Client;
using System.Text;

namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MessageConverterService : IMessageConverterService
    {
        private readonly IGuidService _guidService;
        private readonly JsonSerializerService _jsonConverterService;

        public MessageConverterService(IGuidService guidService, JsonSerializerService jsonConverterService)
        {
            _guidService = guidService ?? throw new ArgumentNullException(nameof(guidService));
            _jsonConverterService = jsonConverterService ?? throw new ArgumentNullException(nameof(jsonConverterService));
        }

        public MqttMessage CreateMqttMessage(MqttApplicationMessageReceivedEventArgs mqttApplicationMessageReceivedEventArgs)
        {
            return new MqttMessage
            {
                Id = _guidService.CreateNewGuid(),
                Timestamp = DateTime.Now,
                Title = mqttApplicationMessageReceivedEventArgs.ApplicationMessage.Topic,
                Body = mqttApplicationMessageReceivedEventArgs.ApplicationMessage.PayloadSegment
            };
        }

        public T? ConvertPayload<T>(MqttMessage mqttMessage)
        {
            var payload = Encoding.UTF8.GetString(mqttMessage.Body);

            if(typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(payload, typeof(T));
            }

            return _jsonConverterService.Deserialize<T>(payload);
        }
    }
}
