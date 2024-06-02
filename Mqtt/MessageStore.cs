using Cooliemint.ApiServer.Services.Messaging;
using System.Text.Json.Nodes;

namespace Cooliemint.ApiServer.Mqtt
{
    public sealed class MessageStore(ValueStore valueStore, IMessageConverterService messageConverterService) : IMessageStore
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

        public void Initialize()
        {
            MessageReceived += (sender, args) =>
            {
                valueStore.AddValue(args.Title, args);

                try
                {
                    var dict = DeconstructJsonString(args.Title, messageConverterService.ConvertPayload<string>(args) ?? string.Empty);
                    foreach (var childItems in dict)
                    {
                        valueStore.AddValue(childItems.Key, childItems.Value);
                    }
                }
                catch (Exception e)
                {

                }
            };
        }

        static Dictionary<string, object> DeconstructJsonString(string parentPath, string jsonString)
        {
            Dictionary<string, object> map = [];

            try
            {
                if (JsonNode.Parse(jsonString) is JsonObject rootObject)
                {
                    foreach (var property in rootObject.AsObject() ?? [])
                    {
                        map.Add(parentPath + "/" + property.Key, property.Value?.ToString() ?? string.Empty);
                        if (property.Value is JsonObject jObject)
                        {
                            foreach (var childItems in DeconstructJsonString(parentPath + "/" + property.Key, jObject.ToString()))
                            {
                                map.Add(childItems.Key, childItems.Value);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return map;
        }
    }
}
