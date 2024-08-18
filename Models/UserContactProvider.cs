namespace Cooliemint.ApiServer.Models
{
    public class UserContactProvider
    {
        public int Id { get; set; }

        public required User User {  get; set; }

        public required ContactProvider ContactProvider { get; set; }
    }
}
