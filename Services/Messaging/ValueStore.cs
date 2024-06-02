﻿using Cooliemint.ApiServer.Mqtt;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class ValueStore
    {
        private readonly IMessageConverterService messageConverterService;
        Dictionary<string, ValueSet> values = new();

        public ValueStore(IMessageConverterService messageConverterService)
        {
            this.messageConverterService = messageConverterService ?? throw new ArgumentNullException(nameof(messageConverterService));
        }

        public void AddValue(string key, object value)
        {
            if (values.ContainsKey(key) && values[key].Value.Equals(value))
            {
                values[key] = new ValueSet(key, value, values[key].CreationTime, DateTime.Now);
                return;
            }

            values[key] = new ValueSet(key, value, DateTime.Now, DateTime.Now);
        }

        public T? GetValue<T>(string key)
        {
            if (!values.ContainsKey(key))
            {
                return default;
            }

            if (values[key].Value is MqttMessage message) 
            {
                return messageConverterService.ConvertPayload<T>(message);
            }

            return (T?) Convert.ChangeType(values[key].Value, typeof(T));
        }

        public object? GetValue(string key)
        {
            if(!values.ContainsKey(key))
            {
                return null;
            }

            return values[key];
        }

        public DateTime? GetCreationDate(string key)
        {
            if(values.TryGetValue(key, out var valueSet))
            {
                return valueSet.CreationTime;
            }

            return null;
        }
    }
}
