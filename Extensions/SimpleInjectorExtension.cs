using Cooliemint.ApiServer.BackgroundServices;
using Cooliemint.ApiServer.Controllers;
using Cooliemint.ApiServer.Models;
using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services;
using Cooliemint.ApiServer.Services.Automation;
using Cooliemint.ApiServer.Services.Factories;
using Cooliemint.ApiServer.Services.Messaging;
using Cooliemint.ApiServer.Services.Messaging.Pushover;
using Cooliemint.ApiServer.Services.Repositories;
using Cooliemint.ApiServer.SmartDeviceDiscovery;
using CoolieMint.WebApp.Services.Notification.Pushover;
using Microsoft.EntityFrameworkCore;

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
            services.RegisterSmartDeviceHandling();

            if (webHostEnvironment.IsLocalDevelopment())
            {
                services.RegisterDeveloperDatabaseService(configurationManager, webHostEnvironment);
            }
            else
            {
                services.RegisterDatabaseServices(configurationManager, webHostEnvironment);
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
            services.AddSingleton<ValueStoreStringReplacementService>();
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
            services.AddSingleton<RuleValidationHandler>();
            services.AddSingleton<RulePartValidator>();
            services.AddSingleton<RulePartValueStoreValueStrategy>();
            services.AddSingleton<RulePartValueStoreValueAgeStrategy>();
            services.AddSingleton<RulePartValidationCache>();

            services.AddScoped<RuleRepository>();
            services.AddSingleton<RuleFactory>();
            services.AddScoped<RulePartRepository>();
            services.AddSingleton<RulePartFactory>();
            services.AddScoped<RuleCommandRepository>();
            services.AddSingleton<RuleCommandFactory>();
            services.AddScoped<ContactProviderRepository>();
            services.AddScoped<NotificationDetailsRepository>();
            services.AddSingleton<NotificationDetailsFactory>();
        }

        public static void RegisterSmartDeviceHandling(this IServiceCollection services)
        {
            services.AddSingleton<SmartDeviceDiscoveryProvider>();
            services.AddSingleton<SmartDeviceFactory>();
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

        public static void RegisterDatabaseServices(this IServiceCollection services, ConfigurationManager configurationManager, IWebHostEnvironment webHostEnvironment)
        {
            var connectionString = configurationManager["ConnectionString"];

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContextFactory<CooliemintDbContext>(
                dbContextOptions =>
                {
                    dbContextOptions.UseMySql(connectionString, serverVersion)
                        .LogTo(Console.WriteLine, LogLevel.Warning);

                    if (webHostEnvironment.IsDevelopment())
                    {
                        dbContextOptions
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                    }
                });
        }

        public static void RegisterDeveloperDatabaseService(this IServiceCollection services, ConfigurationManager configurationManager, IWebHostEnvironment webHostEnvironment)
        {
            var connectionString = configurationManager["ConnectionString"];

            services.AddDbContextFactory<CooliemintDbContext>(dbContextOptions =>
            {
                dbContextOptions
                    .UseSqlite(connectionString);

                if (webHostEnvironment.IsDevelopment())
                {
                    dbContextOptions
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }
            });
        }

        public static void InitializeCooliemintApplication(this IServiceProvider services, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                //services.InitializeDatabase();
            }

            services.GetRequiredService<ConfigurationService>().Initialize();
            services.GetRequiredService<PushoverAccountStore>().Initialize();
            services.GetRequiredService<IMessageStore>().Initialize();
        }

        //public static void InitializeDatabase(this IServiceProvider services)
        //{
        //    using (var scope = services.CreateScope())
        //    {
        //        var factory = scope.ServiceProvider.GetService<IDbContextFactory<CooliemintDbContext>>();
        //        var context = factory?.CreateDbContext();
        //        if (context == null)
        //        {
        //            return;
        //        }

        //        context.Database.EnsureCreated();
        //        if (context.Users.Any())
        //        {
        //            return;
        //        }

        //        context.Users.Add(new User() { Name = "Timo" });
        //        context.Users.Add(new User() { Name = "Nicci" });
        //        context.Notifications.Add(new Notification() { Type = NotificationType.RuleExecuted, Title = "Badezimmerfenster geöffnet" });
        //        context.Notifications.Add(new Notification() { Type = NotificationType.RuleReset, Title = "Badezimmerfenster wieder geschlossen" });
        //        context.Notifications.Add(new Notification() { Type = NotificationType.RuleExecuted, Title = "Waschmaschine läuft" });
        //        context.Notifications.Add(new Notification() { Type = NotificationType.RuleExecuted, Title = "Waschmaschine fertig" });

        //        context.Rules.Add(new RuleModel
        //        {
        //            Description = "",
        //            Name = "Bathroom window checking",
        //            Parts = [
        //                new RulePartModel { Description = "Bathroom window open", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "shellies/shelly17/status/window" , OperandRight = "1" , Operation = RulePartOperation.Equals },
        //                new RulePartModel { Description = "Open for over 1min", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStoreAge, OperandLeft = "shellies/shelly17/status/window" , OperandRight = "00:20:00" , Operation = RulePartOperation.GreaterThanOrEquals  },
        //                new RulePartModel { Description = "Bathroom window closed", Type = RulePartType.ResetCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "shellies/shelly17/status/window" , OperandRight = "0" , Operation = RulePartOperation.Equals}
        //                ],
        //            Commands = []
        //        });

        //        context.Rules.Add(new RuleModel
        //        {
        //            Description = "",
        //            Name = "Waschmaschine läuft",
        //            Parts = [
        //                new RulePartModel { Description = "Waschmaschine läuft", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "shellies/shellyplug5/relay/0/power" , OperandRight = "10" , Operation = RulePartOperation.GreaterThanOrEquals },
        //                new RulePartModel { Description = "Waschmaschine status fertig check", Type = RulePartType.ResetCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "waschmaschine_status" , OperandRight = "warte_auf_fertig" , Operation = RulePartOperation.Equals},
        //                new RulePartModel { Description = "Waschmaschine status fertig check", Type = RulePartType.ResetCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "waschmaschine_status" , OperandRight = "fertig" , Operation = RulePartOperation.Equals}
        //            ],
        //            Commands = [
        //                new RuleCommand("Waschmaschine status läuft", RuleCommandType.SetValueStore ,"waschmaschine_status=läuft" )
        //            ]
        //        });

        //        context.Rules.Add(new RuleModel
        //        {
        //            Description = "",
        //            Name = "Waschmaschine status warte_auf_fertig",
        //            Parts = [
        //                new RulePartModel { Description = "Waschmaschine verbrauch check", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "shellies/shellyplug5/relay/0/power" , OperandRight = "5" , Operation = RulePartOperation.LessThanOrEquals },
        //                new RulePartModel { Description = "Waschmaschine status läuft check", Type = RulePartType.ResetCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "waschmaschine_status" , OperandRight = "läuft" , Operation = RulePartOperation.Equals }
        //            ],
        //            Commands = [
        //                new RuleCommand("Waschmaschine status fertig", RuleCommandType.SetValueStore ,"waschmaschine_status=warte_auf_fertig" )
        //            ]
        //        });

        //        context.Rules.Add(new RuleModel
        //        {
        //            Description = "",
        //            Name = "Waschmaschine fertig",
        //            Parts = [
        //                new RulePartModel { Description = "waschmaschine_status == warte_auf_fertig", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "waschmaschine_status" , OperandRight = "warte_auf_fertig" , Operation = RulePartOperation.Equals },
        //                new RulePartModel { Description = "waschmaschine_status older than 1min", Type = RulePartType.ExecuteCondition, Descriptor = RulePartDescriptor.ValueStoreAge, OperandLeft = "waschmaschine_status" , OperandRight = "00:01:00" , Operation = RulePartOperation.GreaterThanOrEquals},
        //                new RulePartModel { Description = "waschmaschine_status == warte_auf_fertig", Type = RulePartType.ResetCondition, Descriptor = RulePartDescriptor.ValueStore, OperandLeft = "waschmaschine_status" , OperandRight = "warte_auf_fertig" , Operation = RulePartOperation.Equals}
        //            ],
        //            Commands = [
        //                new RuleCommand("Waschmaschine status fertig", RuleCommandType.SetValueStore ,"waschmaschine_status=fertig" )
        //            ]
        //        });

        //        context.SaveChanges();

        //        context.RuleNotifications.Add(new RuleNotification
        //        {
        //            Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster geöffnet")),
        //            Rule = context.Rules.First(r => r.Name.Equals("Bathroom window checking"))
        //        });

        //        context.RuleNotifications.Add(new RuleNotification
        //        {
        //            Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster wieder geschlossen")),
        //            Rule = context.Rules.First(r => r.Name.Equals("Bathroom window checking"))
        //        });

        //        context.RuleNotifications.Add(new RuleNotification
        //        {
        //            Notification = context.Notifications.First(r => r.Title.Equals("Waschmaschine läuft")),
        //            Rule = context.Rules.First(r => r.Name.Equals("Waschmaschine läuft"))
        //        });

        //        context.RuleNotifications.Add(new RuleNotification
        //        {
        //            Notification = context.Notifications.First(r => r.Title.Equals("Waschmaschine fertig")),
        //            Rule = context.Rules.First(r => r.Name.Equals("Waschmaschine fertig"))
        //        });

        //        context.ContactProviders.Add(new ContactProvider
        //        {
        //            Description = "Timo pushover",
        //            Configuration = "{\"ApplicationKey\":\"ac7m2x43uwmxeyui77sr93duteafyp\",\"UserKey\":\"gdm5irtu9yriqrxvqygmkfk9c85r1u\"}",
        //            Type = ContactProviderModelType.Pushover
        //        });

        //        context.SaveChanges();

        //        context.UserContactProviders.Add(new UserContactProvider
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            ContactProvider = context.ContactProviders.First(cp => cp.Description.Equals("Timo pushover"))
        //        });

        //        context.UserNotifications.Add(new UserNotification
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster geöffnet")),
        //            IsActive = true
        //        });

        //        context.UserNotifications.Add(new UserNotification
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster wieder geschlossen")),
        //            IsActive = true
        //        });

        //        context.UserNotifications.Add(new UserNotification
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            Notification = context.Notifications.First(r => r.Title.Equals("Badezimmerfenster wieder geschlossen")),
        //            IsActive = true
        //        });

        //        context.UserNotifications.Add(new UserNotification
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            Notification = context.Notifications.First(r => r.Title.Equals("Waschmaschine fertig")),
        //            IsActive = true
        //        });

        //        context.UserNotifications.Add(new UserNotification
        //        {
        //            User = context.Users.First(u => u.Name.Equals("Timo")),
        //            Notification = context.Notifications.First(r => r.Title.Equals("Waschmaschine läuft")),
        //            IsActive = true
        //        });

        //        context.SaveChanges();

        //        context.NotificationDetails.Add(new NotificationDetails
        //        {
        //            Notification = context.Notifications.First(n => n.Title.Equals("Badezimmerfenster geöffnet")),
        //            Title = "Badezimmerfennster offen",
        //            Description = "Badezimmerfenster wurde länger als 20min geöffnet."
        //        });

        //        context.NotificationDetails.Add(new NotificationDetails
        //        {
        //            Notification = context.Notifications.First(n => n.Title.Equals("Badezimmerfenster wieder geschlossen")),
        //            Title = "Badezimmerfennster geschlossen",
        //            Description = "Badezimmerfenster wurde wieder geschlossen."
        //        });

        //        context.NotificationDetails.Add(new NotificationDetails
        //        {
        //            Notification = context.Notifications.First(n => n.Title.Equals("Waschmaschine läuft")),
        //            Title = "Waschmaschine",
        //            Description = "Waschmaschine läuft"
        //        });

        //        context.NotificationDetails.Add(new NotificationDetails
        //        {
        //            Notification = context.Notifications.First(n => n.Title.Equals("Waschmaschine fertig")),
        //            Title = "Waschmaschine",
        //            Description = "Waschmaschine fertig"
        //        });

        //        context.SaveChanges();
        //    }
        //}
    }
}