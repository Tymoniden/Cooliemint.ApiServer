using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services;
using Cooliemint.ApiServer.Services.Messaging;
using Cooliemint.ApiServer.Services.Messaging.Pushover;
using CoolieMint.WebApp.Services.Notification.Pushover;
using System.Diagnostics;

namespace Cooliemint.ApiServer.Extensions
{
    public static class SimpleInjectorExtension
    {
        public static bool FloodStatus { get; set; }
        public static string LightStatus { get; set; } = string.Empty;

        public static void RegisterCooliemintServices(this IServiceCollection services)
        {
            services.RegisterSharedServices();
            services.RegisterMqttServices();
            services.RegisterMessagingServices();
            services.RegisterBackgroundServices();
        }

        public static void RegisterSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<IGuidService, GuidService>();
            services.AddSingleton<JsonSerializerService>();
            services.AddSingleton<IMessageStore, MessageStore>();
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
        }

        public static void RegisterMessagingServices(this IServiceCollection services)
        {
            services.AddSingleton<IPushOverService, PushOverService>();
            services.AddSingleton<PushoverHttpClient>();
            services.AddSingleton<IPushoverHttpRequestFactory, PushoverHttpRequestFactory>();
            services.AddSingleton<IPushoverHttpContentFactory, PushoverHttpContentFactory>();
            services.AddSingleton<PushoverAccountStore>();
            services.AddSingleton<PushoverMessageFactory>();
        }

        public static void RegisterBackgroundServices(this IServiceCollection services) 
        {
            services.AddHostedService<MqttClientHostedService>();
        }

        public static void RegisterMqttServices(this IServiceCollection services) 
        {
            services.AddSingleton<IMessageConverterService, MessageConverterService>();
        }

        public static void InitializeCooliemintApplication(this IServiceProvider services)
        {
            var pushoverService = services.GetRequiredService<IPushOverService>();
            var messageConverterService = services.GetRequiredService<IMessageConverterService>();

            services.GetRequiredService<IMessageStore>().MessageReceived += (sender, args) => 
            {
                if (args.Title.Equals("shellies/shellyflood1/sensor/flood"))
                {

                    var floodDetected = messageConverterService.ConvertPayload<bool>(args);
                    if (FloodStatus != floodDetected)
                    {
                        FloodStatus = floodDetected;

                        Task.Run(async () =>
                        {
                            Debug.WriteLine("Sending pushover notification");
                            await pushoverService.SendMessage(new AppNotification
                            {
                                Title = "Es ist was passiert",
                                Message = "Wasser " + (floodDetected ? string.Empty : "nicht ") + "im Keller"
                            }, CancellationToken.None);

                        });
                    }
                }

                if (args.Title.Equals("shellies/shellyplug2/relay/0"))
                {
                    var lightStatus = messageConverterService.ConvertPayload<string>(args) ?? string.Empty;
                    if (LightStatus != lightStatus)
                    {
                        LightStatus = lightStatus;

                        Task.Run(async () =>
                        {
                            Debug.WriteLine("Sending pushover notification");
                            await pushoverService.SendMessage(new AppNotification
                            {
                                Title = "Nachtischlampe geändert",
                                Message = "Lampe ist jetzt " + (lightStatus.Equals("on") ? "an" : "aus!")
                            }, CancellationToken.None);
                        });
                    }
                }
            };

            services.GetRequiredService<ConfigurationService>().Initialize();
            services.GetRequiredService<PushoverAccountStore>().Initialize();
        }
    }
}
