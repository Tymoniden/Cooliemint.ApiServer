namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MessageStore : IMessageStore
    {
        private const int limit = 1000;
        private LinkedList<MqttMessage> _messages = new ();
        

        public event EventHandler<MqttMessage>? MessageReceived;

        public void AddMessage(MqttMessage mqttMessage)
        {
            _messages.AddLast(mqttMessage);

            while(_messages.Count > limit)
            {
                _messages.RemoveFirst();
            }

            MessageReceived?.Invoke(null, mqttMessage);
        }
    }
}
