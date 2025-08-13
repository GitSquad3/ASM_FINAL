namespace SIMSWebApp.DatabaseContext.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "Admin";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
