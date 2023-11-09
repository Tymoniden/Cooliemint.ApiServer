// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Cooliemint.ApiServer.Mqtt;

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public sealed class PushoverAccountStore
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly JsonSerializerService _jsonSerializerService;
        private List<PushoverAccountDto> _pushoverAccounts = new();
        private const string ConfigurationFile = "pushover_accounts.json";

        public PushoverAccountStore(IFileSystemService fileSystemService, JsonSerializerService jsonSerializerService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _jsonSerializerService = jsonSerializerService ?? throw new ArgumentNullException(nameof(jsonSerializerService));
        }

        public void Add(PushoverAccountDto pushoverAccount)
        {
            if (pushoverAccount.ApplicationKey == null)
            {
                throw new ArgumentException(nameof(pushoverAccount.ApplicationKey));
            }

            if (pushoverAccount.UserKey == null)
            {
                throw new ArgumentException(nameof(pushoverAccount.UserKey));
            }

            PushoverAccountDto? existingPushoverAccount = null;
            if (pushoverAccount.Id > 0 && _pushoverAccounts.Any(account => account.Id == pushoverAccount.Id))
            {
                existingPushoverAccount = _pushoverAccounts.First(account => account.Id == pushoverAccount.Id);
            }

            if (existingPushoverAccount == null)
            {
                var lastId = _pushoverAccounts.OrderByDescending(account => account.Id).FirstOrDefault()?.Id ?? 0;

                existingPushoverAccount = new PushoverAccountDto
                {
                    Id = ++lastId,
                };

                _pushoverAccounts.Add(existingPushoverAccount);
            }

            existingPushoverAccount.ApplicationKey = pushoverAccount.ApplicationKey;
            existingPushoverAccount.UserKey = pushoverAccount.UserKey;

            PersistAccounts();
        }

        public void Remove(PushoverAccountDto pushoverAccount)
        {
            Remove(pushoverAccount.Id);
        }

        public void Remove(int id)
        {
            var existingAccount = _pushoverAccounts.FirstOrDefault(account => account.Id == id);

            if (existingAccount == null)
            {
                return;
            }

            _pushoverAccounts.Remove(existingAccount);
            PersistAccounts();
        }

        public PushoverAccountDto? Get(int id)
        {
            return _pushoverAccounts.FirstOrDefault(account => account.Id == id);
        }

        public List<PushoverAccountDto> GetAll()
        {
            return _pushoverAccounts;
        }

        public void Initialize()
        {
            try
            {
                var storedAccounts = _jsonSerializerService.Deserialize<List<PushoverAccountDto>>(
                    _fileSystemService.ReadAllText(ConfigurationFile));

                if (storedAccounts?.Any() == true)
                {
                    _pushoverAccounts.AddRange(storedAccounts);
                }
            }
            catch
            {
                // just swallow, it might be the first time to read this file
            }
        }

        private void PersistAccounts()
        {
            _fileSystemService.WriteAllText(ConfigurationFile, _jsonSerializerService.Serialize(_pushoverAccounts));
        }
    }
}
