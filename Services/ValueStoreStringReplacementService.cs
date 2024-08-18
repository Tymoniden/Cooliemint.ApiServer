using Cooliemint.ApiServer.Services.Messaging;
using System.Text.RegularExpressions;

namespace Cooliemint.ApiServer.Services
{
    public class ValueStoreStringReplacementService(ValueStore valueStore)
    {
        private const string _startingToken = @"@>";
        private const string _endingToken = @"<@";

        public string InsertValueStoreValues(string input)
        {
            if (!HasToken(input))
                return input;

            Dictionary<string, string> matchesToReplace = [];
            foreach (Match match in Regex.Matches(input, $"{_startingToken}(.*?){_endingToken}", RegexOptions.IgnoreCase))
            {
                if (matchesToReplace.ContainsKey(match.Groups["0"].Value))
                    continue;

                var value = valueStore.GetValue<string>(match.Groups["1"].Value);
                if (value == null)
                    continue;

                matchesToReplace.Add(match.Groups["0"].Value, value ?? string.Empty);
            }
            
            foreach (var kvp in matchesToReplace)
            {
                input = input.Replace(kvp.Key, kvp.Value);
            }

            return input;
        }

        private bool HasToken(string input)
        {
            return input.IndexOf(_startingToken) != -1 && input.IndexOf(_startingToken) != -1;
        }
    }
}
