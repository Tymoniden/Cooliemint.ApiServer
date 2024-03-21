namespace Cooliemint.ApiServer.Services
{
    public class ValueStore
    {
        private readonly Dictionary<string, string> _valueStore = new();

        public void SetValue(string key, string value)
        {
            if (!_valueStore.ContainsKey(key))
            {
                _valueStore.Add(key, value);
            }
            else
            {
                _valueStore[key] = value;
            }
        }

        public string GetValue(string key) => _valueStore.ContainsKey(key) ? _valueStore[key] : string.Empty;
    }
}
