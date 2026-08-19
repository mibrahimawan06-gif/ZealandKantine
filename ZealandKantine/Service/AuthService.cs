using ZealandKantine.models;

namespace ZealandKantine.Service
{
    /// <summary>
    /// Slaar brugere op i konfigurationen og haandterer login og logud
    /// via sessionen.
    ///
    /// BEGRAENSNING: adgangskoder gemmes i klartekst i appsettings.json og
    /// sammenlignes direkte. Det er tilstraekkeligt til en prototype, men
    /// en produktionsversion skal bruge hashede kodeord, fx via
    /// ASP.NET Core Identity.
    /// </summary>
    public class AuthService
    {
        private readonly List<AppUser> _users;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _users = configuration.GetSection("Users").Get<List<AppUser>>() ?? new List<AppUser>();
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        /// <summary>
        /// Validerer loginoplysninger og saetter sessionen. Returnerer false
        /// ved ukendt bruger eller forkert adgangskode.
        /// </summary>
        public bool TryLogin(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user == null)
            {
                return false;
            }

            Session.SetString("IsLoggedIn", "true");
            Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");
            Session.SetString("Username", user.Username);
            Session.SetString("UserRole", user.Role);

            return true;
        }

        public void Logout() => Session.Clear();

        public bool IsLoggedIn() => Session.GetString("IsLoggedIn") == "true";

        public bool IsAdmin() => Session.GetString("IsAdmin") == "true";
    }
}
