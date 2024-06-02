namespace Cooliemint.ApiServer.Models
{
    public class UserContactProvider
    {
        public int Id { get; set; }

        public required virtual User User {  get; set; }

        public required virtual ContactProvider ContactProvider { get; set; }
    }
}
