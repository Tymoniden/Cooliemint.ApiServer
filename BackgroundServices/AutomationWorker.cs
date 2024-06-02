
namespace Cooliemint.ApiServer.BackgroundServices
{
    public class AutomationWorker : BackgroundService
    {
        private readonly ILogger<AutomationWorker> logger;
        private readonly AutomationQueueManager automationQueueManager;
        private readonly IServiceProvider serviceProvider;

        public AutomationWorker(ILogger<AutomationWorker> logger, AutomationQueueManager automationQueueManager, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.automationQueueManager = automationQueueManager ?? throw new ArgumentNullException(nameof(automationQueueManager));
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting " + nameof(AutomationWorker) + " is stopping.");
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            
            try
            {
                await using(var scope = serviceProvider.CreateAsyncScope())
                {
                    await automationQueueManager.Initialize();

                    while (await timer.WaitForNextTickAsync(stoppingToken))
                    {
                        await automationQueueManager.ExecuteQueue(stoppingToken);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, nameof(AutomationWorker) + " is stopping.");
            }
        }
    }
}
