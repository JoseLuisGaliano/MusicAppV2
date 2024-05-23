namespace MusicApp.Database.Models
{
    internal class User
    {
        internal int UserID { get; set; }
        internal string Username { get; set; }
        internal string Email { get; set; }
        internal DateTime DateJoined { get; set; }
        internal bool IsActive { get; set; }
        internal string ProfilePicture { get; set; }
        internal string SubscriptionPlan { get; set; }
        internal string HashedPassword { get; set; }
        internal string Salt { get; set; }
    }
}
