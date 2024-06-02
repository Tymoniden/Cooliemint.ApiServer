using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Messaging.Pushover;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class NotificationService(IDbContextFactory<CooliemintDbContext> dbContextFactory, 
        IPushOverService pushoverService, 
        JsonSerializerService jsonSerializerService, 
        PushoverMessageFactory pushoverMessageFactory)
    {
        public async Task Notify(Rule rule, CancellationToken cancellationToken)
        {
            using var ctx = await dbContextFactory.CreateDbContextAsync();
            
            var contactProviders = from rn in ctx.RuleNotifications
                    join un in ctx.UserNotifications on rn.Rule.Id equals un.NotificationId
                    join ucp in ctx.UserContactProviders on un.UserId equals ucp.User.Id
                    join cp in ctx.ContactProviders on ucp.ContactProvider.Id equals cp.Id
                    join nd in ctx.NotificationDetails on un.NotificationId equals nd.Notification.Id
                    where rn.Rule.Id == rule.Id && un.IsActive
                    select new { ContactProvider = cp, NotificationDetails = nd };
            
            foreach(var contactProvider in contactProviders)
            {
                await CallContactProvider(contactProvider.ContactProvider, contactProvider.NotificationDetails, cancellationToken);
            }
        }

        public async Task CallContactProvider(ContactProvider contactProvider, NotificationDetails notificationDetails, CancellationToken cancellationToken)
        {
            switch (contactProvider.Type)
            {
                case ContactProviderType.Email:
                    break;
                case ContactProviderType.Pushover:
                    var account = jsonSerializerService.Deserialize<PushoverAccountDto>(contactProvider.Configuration);
                    var message = pushoverMessageFactory.CreateMessage(notificationDetails.Title, notificationDetails.Description);

                    if (account != null && message != null)
                    {    
                        await pushoverService.SendMessageToAccount(message, [account], cancellationToken);
                    }
                    
                    break;
            }
        }
    }
}
