using Cooliemint.ApiServer.BackgroundServices;
using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services;
using Cooliemint.ApiServer.Services.Factories;
using Cooliemint.ApiServer.Services.Messaging;
using Cooliemint.ApiServer.Services.Messaging.Pushover;
using Cooliemint.ApiServer.Services.Repositories;
using CoolieMint.WebApp.Services.Notification.Pushover;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Cooliemint.ApiServer.Extensions
{
    public static class SimpleInjectorExtension
    {
        public static void RegisterCooliemintServices(this IServiceCollection services, ConfigurationManager configurationManager, IWebHostEnvironment webHostEnvironment)
        {
            services.RegisterSharedServices();
            services.RegisterMqttServices();
            services.RegisterMessagingServices();
            services.RegisterBackgroundServices(webHostEnvironment);
            services.RegisterAutomationServices();
            services.RegisterApiServices();

            if (webHostEnvironment.IsLocalDevelopment())
            {
                services.RegisterDeveloperDatabaseService(configurationManager);
            }
            else
            {
                services.RegisterDatabaseServices(configurationManager);
            }
        }

        public static void RegisterSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<IGuidService, GuidService>();
            services.AddSingleton<JsonSerializerService>();
            services.AddSingleton<IMessageStore, MessageStore>();
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<ValueStore>();
        }

        public static void RegisterMessagingServices(this IServiceCollection services)
        {
            services.AddSingleton<IPushOverService, PushOverService>();
            services.AddSingleton<PushoverHttpClient>();
            services.AddSingleton<IPushoverHttpRequestFactory, PushoverHttpRequestFactory>();
            services.AddSingleton<IPushoverHttpContentFactory, PushoverHttpContentFactory>();
            services.AddSingleton<PushoverAccountStore>();
            services.AddSingleton<PushoverMessageFactory>();
            services.AddSingleton<NotificationService>();
        }

        public static void RegisterAutomationServices(this IServiceCollection services)
        {
            services.AddSingleton<MqttMessageSenderService>();
        }

        public static void RegisterBackgroundServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            services.AddHostedService<AutomationWorker>();
            services.AddSingleton<AutomationQueueManager>();

            if (webHostEnvironment.IsLocalDevelopment())
            {
                return;
            }
            services.AddHostedService<MqttClientHostedService>();
        }

        public static void RegisterApiServices(this IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddSingleton<UserFactory>();
            services.AddScoped<NotificationRepository>();
            services.AddSingleton<NotificationFactory>();
            services.AddScoped<UserNotificationRepository>();
            services.AddSingleton<UserNotificationFactory>();
        }

        public static void RegisterMqttServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageConverterService, MessageConverterService>();
        }

        public static void RegisterDatabaseServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var connectionString = configurationManager["ConnectionString"];

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContextFactory<Models.CooliemintDbContext>(
                dbContextOptions =>
                {
                    dbContextOptions.UseMySql(connectionString, serverVersion);

                    if (Debugger.IsAttached)
                    {
                        dbContextOptions
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                    }
                });
        }

        public static void RegisterDeveloperDatabaseService(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var connectionString = configurationManager["ConnectionString"];

            services.AddDbContextFactory<Models.CooliemintDbContext>(dbContextOptions =>
            {
                dbContextOptions
                    .UseSqlite(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
        }

        public static void InitializeCooliemintApplication(this IServiceProvider services, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                services.InitializeDatabase();
            }

            services.GetRequiredService<ConfigurationService>().Initialize();
            services.GetRequiredService<PushoverAccountStore>().Initialize();
            services.GetRequiredService<IMessageStore>().Initialize();

            //var pushoverService = services.GetRequiredService<IPushOverService>();
            //var messageConverterService = services.GetRequiredService<IMessageConverterService>();
            //var valueStore = services.GetRequiredService<ValueStore>();

            //services.GetRequiredService<IMessageStore>().MessageReceived += (sender, args) =>
            //{
            //    valueStore.AddValue(args.Title, args);
            //};

            //var jsonSerializerService = services.GetRequiredService<JsonSerializerService>();

            //services.GetRequiredService<IMessageStore>().MessageReceived += (sender, args) =>
            //{
            //try
            //{
            //    var dict = DeconstructJsonString(args.Title, messageConverterService.ConvertPayload<string>(args) ?? string.Empty);
            //    foreach (var childItems in dict)
            //    {
            //        valueStore.AddValue(childItems.Key, childItems.Value);
            //    }
            //}
            //catch(Exception e)
            //{

            //}

            //if (args.Title.Equals("shellies/shellyflood1/sensor/flood"))
            //{
            //    var floodDetected = messageConverterService.ConvertPayload<bool>(args);
            //    if (FloodStatus != floodDetected)
            //    {
            //        FloodStatus = floodDetected;

            //        Task.Run(async () =>
            //        {
            //            Debug.WriteLine("Sending pushover notification");
            //            await pushoverService.SendMessage(new AppNotification
            //            {
            //                Title = "Es ist was passiert",
            //                Message = "Wasser " + (floodDetected ? string.Empty : "nicht ") + "im Keller"
            //            }, CancellationToken.None);

            //        });
            //    }
            //}

            //if (args.Title.Equals("shellies/shellyplug2/relay/0"))
            //{
            //    var lightStatus = messageConverterService.ConvertPayload<string>(args) ?? string.Empty;
            //    if (LightStatus != lightStatus)
            //    {
            //        LightStatus = lightStatus;

            //        Task.Run(async () =>
            //        {
            //            Debug.WriteLine("Sending pushover notification");
            //            await pushoverService.SendMessage(new AppNotification
            //            {
            //                Title = "Nachtischlampe geändert",
            //                Message = "Lampe ist jetzt " + (lightStatus.Equals("on") ? "an" : "aus!")
            //            }, CancellationToken.None);
            //        });
            //    }
            //}
            //};
        }

        public static void InitializeDatabase(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetService<IDbContextFactory<CooliemintDbContext>>();
                var context = factory?.CreateDbContext();
                if (context == null)
                {
                    return;
                }

                context.Database.EnsureCreated();
                if (context.Users.Any())
                {
                    return;
                }

                context.Users.Add(new User() { Name = "Timo" });
                context.Users.Add(new User() { Name = "Nicci" });
                context.Notifications.Add(new Notification() { Title = "Badezimmerfenster geöffnet" });
                context.Notifications.Add(new Notification() { Title = "Küchenschranktür offen" });

                context.Rules.Add(new Rule
                {
                    Description = "",
                    Name = "Bathroom window checking",
                    Parts = [
                        new RulePart { Description = "Bathroom window open", Descriptor = RulePartDescriptor.ValueStore, Value = "shellies/shelly17/status/window=1" },
                        new RulePart { Description = "Open for over 1min", Descriptor = RulePartDescriptor.ValueStoreAge, Value = "shellies/shelly17/status/window=00:20:00" }
                        ],
                    Commands = [
                        //new RuleCommand ("Send Notification", RuleCommandType.Notification, "Badezimmer=Badezimmerfenster wurde länger als 20 min aufgelassen!")
                    ],
                    ResetConditions = [
                        new ResetCondition {
                            RulePart = new RulePart { Description = "Bathroom window closed", Descriptor = RulePartDescriptor.ValueStore, Value = "shellies/shelly17/status/window=0" } 
                        }
                    ]
                });

                context.SaveChanges();

                context.RuleNotifications.Add(new RuleNotification
                {
                    Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster geöffnet")),
                    Rule = context.Rules.First(r => r.Name.Equals("Bathroom window checking"))
                });

                context.ContactProviders.Add(new ContactProvider
                {
                    Description = "Timo pushover",
                    Configuration = "{\"ApplicationKey\":\"ac7m2x43uwmxeyui77sr93duteafyp\",\"UserKey\":\"gdm5irtu9yriqrxvqygmkfk9c85r1u\"}"
                });

                context.SaveChanges();

                context.UserContactProviders.Add(new UserContactProvider
                {
                    User = context.Users.First(u => u.Name.Equals("Timo")),
                    ContactProvider = context.ContactProviders.First(cp => cp.Description.Equals("Timo pushover"))
                });

                context.UserNotifications.Add(new UserNotification
                {
                    UserId = context.Users.First(u => u.Name.Equals("Timo")).Id,
                    NotificationId = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster geöffnet")).Id,
                    IsActive = true
                });

                context.SaveChanges();

                context.NotificationDetails.Add(new NotificationDetails
                {
                    Notification = context.Notifications.First(n => n.Title.Equals("Badezimmerfenster geöffnet")),
                    Title = "Badezimmerfennster offen",
                    Description = "Badezimmerfenster wurde länger als 20min geöffnet."
                });

                context.SaveChanges();
            }
        }

        //static Dictionary<string, object> DeconstructJsonString(string parentPath, string jsonString)
        //{
        //    Dictionary<string, object> map = [];

        //    try
        //    {
        //        if (JsonNode.Parse(jsonString) is JsonObject rootObject)
        //        {
        //            foreach (var property in rootObject.AsObject() ?? [])
        //            {
        //                map.Add(parentPath + "/" + property.Key, property.Value?.ToString() ?? string.Empty);
        //                if (property.Value is JsonObject jObject)
        //                {
        //                    foreach (var childItems in DeconstructJsonString(parentPath + "/" + property.Key, jObject.ToString()))
        //                    {
        //                        map.Add(childItems.Key, childItems.Value);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    return map;
        //}
    }
}