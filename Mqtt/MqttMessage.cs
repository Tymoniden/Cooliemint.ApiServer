namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MqttMessage
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = string.Empty;
        public ArraySegment<byte> Body { get; set; }
    }
}
