// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Services.Messaging.Pushover
{
    public sealed class PushoverAccountStore
    {
        private List<PushoverAccountDto> _pushoverAccounts = new();

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
        }

        public PushoverAccountDto? Get(int id)
        {
            return _pushoverAccounts.FirstOrDefault(account => account.Id == id);
        }

        public List<PushoverAccountDto> GetAll()
        {
            return _pushoverAccounts;
        }
    }
}
