namespace MusicApp.Database.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePicture { get; set; }
        public string SubscriptionPlan { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
