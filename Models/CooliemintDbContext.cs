using Cooliemint.ApiServer.SmartDeviceDiscovery;
using Cooliemint.ApiServer.ValueHistory;
using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Models
{
    public class CooliemintDbContext(DbContextOptions<CooliemintDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<NotificationModel> Notifications { get; set; }
        public virtual DbSet<NotificationDetailsModel> NotificationDetails { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<RuleModel> Rules { get; set; }
        public virtual DbSet<RulePartModel> RuleParts { get; set; }
        public virtual DbSet<RuleCommandModel> RuleCommands { get; set; }
        public virtual DbSet<RuleNotification> RuleNotifications { get; set; }
        public virtual DbSet<ContactProvider> ContactProviders { get; set; }
        public virtual DbSet<UserContactProvider> UserContactProviders { get; set; }
        public virtual DbSet<SmartDeviceModel> SmartDevices { get; set; }
        public virtual DbSet<ValueHistoryEntryModel> ValueHistoryEntries { get; set; }
    }
}