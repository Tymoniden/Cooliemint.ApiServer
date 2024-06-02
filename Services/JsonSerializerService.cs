using Newtonsoft.Json;

namespace Cooliemint.ApiServer.Services
{
    public sealed class JsonSerializerService
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string Serialize(object obj, SerializerSettings serializerSettings)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T? Deserialize<T>(string payload)
        {
            return JsonConvert.DeserializeObject<T>(payload);
        }
    }
}
