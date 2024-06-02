using Microsoft.EntityFrameworkCore;

namespace Cooliemint.ApiServer.Models
{
    public class CooliemintDbContext(DbContextOptions<CooliemintDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationDetails> NotificationDetails { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<RulePart> RuleParts { get; set; }
        public virtual DbSet<RuleCommand> RuleCommands { get; set; }
        public virtual DbSet<ResetCondition> ResetConditions { get; set; }
        public virtual DbSet<RuleNotification> RuleNotifications { get; set; }
        public virtual DbSet<ContactProvider> ContactProviders { get; set; }
        public virtual DbSet<UserContactProvider> UserContactProviders { get; set; }
    }
}