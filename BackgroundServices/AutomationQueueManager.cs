using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services.Automation;
using Cooliemint.ApiServer.Services.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.BackgroundServices
{
    public class AutomationQueueManager(ILogger<AutomationQueueManager> logger,
        MqttMessageSenderService mqttMessageSenderService,
        IPushOverService pushOverService,
        IDbContextFactory<CooliemintDbContext> dbContextFactory,
        NotificationService notificationService,
        RuleValidationHandler ruleEvaluationHandler,
        ValueStore valueStore)
    {
        public async Task ExecuteQueue(CancellationToken cancellationToken)
        {
            using (var ctx = dbContextFactory.CreateDbContext())
            {
                var anythingChanged = false;
                await foreach (var rule in ctx
                    .Rules
                    .Include(r => r.Parts)
                    .Include(r => r.Commands)
                    .Where(r => !r.IsExecuted)
                    .AsAsyncEnumerable()
                    .WithCancellation(cancellationToken))
                {
                    try
                    {
                        if (ruleEvaluationHandler.ExecuteConditionsMatch(rule))
                        {
                            logger.LogInformation($"Rule: {rule.Name} was successful.");
                            foreach (var command in rule.Commands)
                            {
                                logger.LogInformation($"Executing {rule.Name}:{command.Name}");
                                // TODO parallize through order index: 0,1,2,2,2,3 ....
                                await ExecuteCommand(command);
                            }

                            rule.IsExecuted = true;
                            rule.LastExecuted = DateTime.UtcNow;
                            anythingChanged = true;
                            await notificationService.NotifyExecution(rule.Id, cancellationToken);
                        }
                    }
                    catch
                    {

                    }
                }

                if(anythingChanged)
                {
                    await ctx.SaveChangesAsync();
                }
            }

            using (var ctx = dbContextFactory.CreateDbContext())
            {
                var anythingReset = false;
                await foreach (var rule in ctx
                    .Rules
                    .Include(r => r.Parts)
                    .Include(r => r.Commands)
                    .Where(r => r.IsExecuted && r.Parts.Any(p => p.Type == RulePartType.ResetCondition))
                    .AsAsyncEnumerable()
                    .WithCancellation(cancellationToken))
                {
                    if (ruleEvaluationHandler.ResetConditionsMatch(rule))
                    {
                        rule.IsExecuted = false;
                        anythingReset = true;
                        await notificationService.NotifyReset(rule.Id, cancellationToken);
                        break;
                    }
                }

                if (anythingReset)
                {
                    await ctx.SaveChangesAsync();
                }
            }
        }

        async Task ExecuteCommand(RuleCommandModel command)
        {
            switch(command.CommandType)
            {
                case RuleCommandType.Mqtt:
                    var segments = command.Value.Split("=");
                    mqttMessageSenderService.SendMessage(segments[0], segments[1]);
                    break;
                case RuleCommandType.SetValueStore:
                    var valueStoreSegments = command.Value.Split("=");
                    valueStore.AddValue(valueStoreSegments[0], valueStoreSegments[1]);
                    break;
                case RuleCommandType.Notification:
                    var notificationSegments = command.Value.Split("=");
                    await pushOverService.SendMessage(new AppNotification
                    {
                        Title = notificationSegments[0],
                        Message = notificationSegments[1]
                    }, CancellationToken.None);
                    break;

            }

            await Task.CompletedTask;
        }
    }
}
