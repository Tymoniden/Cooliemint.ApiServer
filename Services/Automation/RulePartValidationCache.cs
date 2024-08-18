namespace Cooliemint.ApiServer.Services.Automation
{
    public class RulePartValidationCache
    {
        readonly Dictionary<int, RulePartValidtion> cache = [];

        public void SetValue(int id, bool isValid)
        {
            if(!cache.ContainsKey(id))
            {
                cache.Add(id, new(id, isValid, DateTime.Now));
            }
            else
            {
                if (cache[id].IsValid != isValid)
                {
                    var entry = cache[id];
                    entry.IsValid = isValid;
                    entry.Modified = DateTime.Now;
                }
            }
        }

        public bool GetValue(int id)
        {
            if(cache.TryGetValue(id, out var entry))
            {
                return entry.IsValid;
            }
            
            return false;
        }
    }
}
