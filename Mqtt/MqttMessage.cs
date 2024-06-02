using System.Text;

namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MqttMessage
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; } = string.Empty;
        public ArraySegment<byte> Body { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;

            if(ReferenceEquals(this, obj)) return true;

            if(obj is MqttMessage msg) 
            {
                return msg.Title.Equals(Title) && msg.Body.SequenceEqual(Body);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
