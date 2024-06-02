using System.Text;

namespace Cooliemint.ApiServer.Mqtt
{
    public class MqttMessageSenderService
    {
        public event EventHandler<MqttMessage>? SendMessageEvent;

        public void SendMessage(string topic, string body, bool retained = false)
        {
            SendMessageEvent?.Invoke(this, new MqttMessage { Title = topic, Body = Encoding.UTF8.GetBytes(body) });
        }
    }
}
