namespace Cooliemint.ApiServer.Mqtt
{
    public interface IMessageStore
    {
        public event EventHandler<MqttMessage>? MessageReceived;

        public void AddMessage(MqttMessage mqttMessage);
        void Initialize();
    }
}
