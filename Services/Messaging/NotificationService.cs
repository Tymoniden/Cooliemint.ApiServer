using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Services.Messaging.Pushover;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Services.Messaging
{
    public class NotificationService(ILogger<NotificationService> logger,
        IDbContextFactory<CooliemintDbContext> dbContextFactory, 
        IPushOverService pushoverService, 
        JsonSerializerService jsonSerializerService, 
        PushoverMessageFactory pushoverMessageFactory, 
        ValueStoreStringReplacementService valueStoreStringReplacementService)
    {
        public Task NotifyExecution(int ruleId, CancellationToken cancellationToken)
        {
            return Notify(ruleId, NotificationType.RuleExecuted, cancellationToken);
        }

        public Task NotifyReset(int ruleId, CancellationToken cancellationToken)
        {
            return Notify(ruleId, NotificationType.RuleReset, cancellationToken);
        }

        public async Task Notify(int ruleId, NotificationType type, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Notifing rule: {ruleId}");
            using var ctx = await dbContextFactory.CreateDbContextAsync();

            var contactProviders = ctx.Notifications
                .Include(n => n.RuleNotifications)
                    .ThenInclude(rn => rn.Rule)
                .Include(n => n.UserNotifications)
                    .ThenInclude(un => un.Notification)
                .Include(n => n.UserNotifications)
                    .ThenInclude(un => un.User)
                        .ThenInclude(u => u.ContactProviders)
                            .ThenInclude(cp => cp.ContactProvider)
                .Where(n =>
                    n.RuleNotifications.Any(r => r.Rule.Id == ruleId) &&
                    n.UserNotifications.Any(un => un.IsActive && un.Notification.Type == type))
                .SelectMany(n => n.UserNotifications)
                .SelectMany(un => un.User.ContactProviders)
                .Select(cp => cp.ContactProvider)
                .Distinct()
                .ToList();

            var notificationDetails = ctx.Notifications
                .Include(n => n.Details)
                .Include(n => n.RuleNotifications)
                    .ThenInclude(rn => rn.Rule)
                .Include(n => n.UserNotifications)
                    .ThenInclude(un => un.Notification)
                .Include(n => n.UserNotifications)
                .Where(n =>
                    n.RuleNotifications.Any(r => r.Rule.Id == ruleId) &&
                    n.UserNotifications.Any(un => un.IsActive && un.Notification.Type == type))
                .SelectMany(n => n.UserNotifications)
                .SelectMany(un => un.Notification.Details)
                .Distinct()
                .ToList();

            logger.LogInformation($"found #{contactProviders.Count()} contactProviders.");
            foreach (var notificationDetail in notificationDetails)
            {
                logger.LogInformation($"Notifing: {ruleId}:{notificationDetail.Title}");
                foreach (var contactProvider in contactProviders)
                {
                    logger.LogInformation($"Notifing: {notificationDetail.Title}->{contactProvider.Description}");
                    await CallContactProvider(contactProvider, notificationDetail, cancellationToken);
                }
            }
        }

        public async Task CallContactProvider(ContactProvider contactProvider, NotificationDetailsModel notificationDetails, CancellationToken cancellationToken)
        {
            switch (contactProvider.Type)
            {
                case ContactProviderModelType.Email:
                    break;
                case ContactProviderModelType.Pushover:
                    var account = jsonSerializerService.Deserialize<PushoverAccountDto>(contactProvider.Configuration);
                    var message = pushoverMessageFactory.CreateMessage(notificationDetails.Title, valueStoreStringReplacementService.InsertValueStoreValues(notificationDetails.Description));
                    logger.LogInformation($"sending message: [{message.Title}] to [{account?.Id}]");
                    if (account != null && message != null)
                    {    
                        await pushoverService.SendMessageToAccount(message, [account], cancellationToken);
                    }
                    
                    break;
            }
        }
    }
}
