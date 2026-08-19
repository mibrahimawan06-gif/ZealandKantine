namespace ZealandKantine.models
{
    /// <summary>
    /// En brugerkonto til systemet. Konfigureres i appsettings.json,
    /// saa loginoplysninger ikke staar i kildekoden.
    /// </summary>
    public class AppUser
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool IsAdmin => Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }
}
