using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Kvittering for en gennemfoert ordre. Viser de priser, der blev
    /// laast fast ved bestillingen - ikke de aktuelle priser i menuen.
    /// </summary>
    public class ConfirmationModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly AuthService _authService;

        public ConfirmationModel(OrderService orderService,
            AuthService authService)
        {
            _orderService = orderService;
            _authService = authService;
        }

        public Order? Order { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            Order = _orderService.GetOrder(id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
