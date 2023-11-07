using MQTTnet.Client;

namespace Cooliemint.ApiServer.Mqtt
{
    public interface IMessageConverterService
    {
        T? ConvertPayload<T>(MqttMessage mqttMessage);
        MqttMessage CreateMqttMessage(MqttApplicationMessageReceivedEventArgs mqttApplicationMessageReceivedEventArgs);
    }
}