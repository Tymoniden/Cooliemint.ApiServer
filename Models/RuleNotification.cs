namespace Cooliemint.ApiServer.Models
{
    public class RuleNotification
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }
        
        public virtual required RuleModel Rule { get; set; }
        
        public virtual required NotificationModel Notification { get; set; }
    }
}
