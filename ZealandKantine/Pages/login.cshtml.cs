using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    public class loginModel : PageModel
    {
        private readonly AuthService _authService;

        public loginModel(AuthService authService)
        {
            _authService = authService;
        }

        // BindProperty binder formularens felter til disse properties,
        // saa vaerdierne er tilgaengelige i OnPost
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            // Allerede logget ind - ingen grund til at vise loginsiden igen
            if (_authService.IsLoggedIn())
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (_authService.TryLogin(Username, Password))
            {
                return RedirectToPage("/Index");
            }

            // Samme besked uanset om brugernavnet findes eller ej, saa siden
            // ikke afsloerer hvilke brugernavne der er gyldige
            ErrorMessage = "Ugyldigt brugernavn eller adgangskode.";
            return Page();
        }
    }
}
