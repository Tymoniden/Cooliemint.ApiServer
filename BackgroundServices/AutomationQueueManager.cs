using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cooliemint.ApiServer.BackgroundServices
{
    public class AutomationQueueManager
    {
        private readonly ValueStore valueStore;
        private readonly MqttMessageSenderService mqttMessageSenderService;
        private readonly IPushOverService pushOverService;
        private readonly IDbContextFactory<CooliemintDbContext> dbContextFactory;
        private readonly NotificationService notificationService;

        public AutomationQueueManager(ValueStore valueStore, 
            MqttMessageSenderService mqttMessageSenderService, 
            IPushOverService pushOverService, 
            IDbContextFactory<CooliemintDbContext> dbContextFactory,
            NotificationService notificationService)
        {
            this.valueStore = valueStore ?? throw new ArgumentNullException(nameof(valueStore));
            this.mqttMessageSenderService = mqttMessageSenderService ?? throw new ArgumentNullException(nameof(mqttMessageSenderService));
            this.pushOverService = pushOverService ?? throw new ArgumentNullException(nameof(pushOverService));
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public async Task Initialize()
        {
            
        }

        public async Task ExecuteQueue(CancellationToken cancellationToken)
        {
            using (var ctx = dbContextFactory.CreateDbContext())
            {
                var anythingChanged = false;
                await foreach (var rule in ctx
                    .Rules
                    .Include(r => r.Parts)
                    .Include(r => r.Commands)
                    .Include(r => r.ResetConditions)
                    .Where(r => !r.IsExecuted)
                    .AsAsyncEnumerable()
                    .WithCancellation(cancellationToken))
                {
                    var ruleWasSuccessful = true;

                    foreach (var rulePart in rule.Parts)
                    {
                        if (!EvaluateRule(rulePart))
                        {
                            ruleWasSuccessful = false;
                            break;
                        }
                    }

                    if (ruleWasSuccessful)
                    {
                        foreach (var command in rule.Commands)
                        {
                            Debug.WriteLine($"Executing {rule.Name}:{command.Name}");
                            // TODO parallize through order index: 0,1,2,2,2,3 ....
                            await ExecuteCommand(command);
                        }

                        rule.IsExecuted = true;
                        rule.LastExecuted = DateTime.UtcNow;
                        anythingChanged = true;
                        await notificationService.Notify(rule, cancellationToken);
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
                    .Include(r => r.ResetConditions)
                    .Include(r => r.ResetConditions)
                        .ThenInclude(rc => rc.RulePart)
                    .Where(r => r.IsExecuted && r.ResetConditions != null)
                    .AsAsyncEnumerable()
                    .WithCancellation(cancellationToken))
                {
                    foreach(var resetCondition in rule.ResetConditions.Where(condition => condition?.RulePart != null))
                    {
                        if (EvaluateRule(resetCondition.RulePart!))
                        {
                            rule.IsExecuted = false;
                            anythingReset = true;
                            break;
                        }
                    }
                }

                if (anythingReset)
                {
                    await ctx.SaveChangesAsync();
                }
            }
        }

        async Task ExecuteCommand(RuleCommand command)
        {
            switch(command.CommandType)
            {
                case RuleCommandType.Mqtt:
                    var segments = command.Value.Split("=");
                    mqttMessageSenderService.SendMessage(segments[0], segments[1]);
                    break;
                case RuleCommandType.SetValueStore:
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

        bool EvaluateRule(RulePart rulePart)
        {
            var segments = rulePart.Value.Split("=");

            switch (rulePart.Descriptor)
            {
                case RulePartDescriptor.ValueStore:
                    return valueStore.GetValue<string>(segments[0])?.Equals(segments[1]) ?? false;

                case RulePartDescriptor.ValueStoreAge:
                    if(DateTime.Now.Subtract(TimeSpan.Parse(segments[1])) >= valueStore.GetCreationDate(segments[0]))
                    {
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }
    }

    public record WindowContactDto(int Window, int? Battery, int? Illuminance, int? Rotation) 
    {
        public bool IsOpen => Window == 1;
    }
}
