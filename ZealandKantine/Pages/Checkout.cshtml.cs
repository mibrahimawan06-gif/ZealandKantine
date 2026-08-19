using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Kassen. Her omsaettes den midlertidige kurv til en bindende ordre
    /// i databasen, registreret paa et medarbejdernummer.
    /// </summary>
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly PriceService _priceService;
        private readonly OrderService _orderService;
        private readonly AuthService _authService;

        public CheckoutModel(
            CartService cartService,
            ProductService productService,
            PriceService priceService,
            OrderService orderService,
            AuthService authService)
        {
            _cartService = cartService;
            _productService = productService;
            _priceService = priceService;
            _orderService = orderService;
            _authService = authService;
        }

        [BindProperty]
        public string EmployeeNumber { get; set; } = string.Empty;

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal Total { get; set; }
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            LoadCart();

            if (CartItems.Count == 0)
            {
                return RedirectToPage("/Cart");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            LoadCart();

            if (string.IsNullOrWhiteSpace(EmployeeNumber))
            {
                ErrorMessage = "Indtast dit medarbejdernummer.";
                return Page();
            }

            var order = _orderService.CreateOrder(EmployeeNumber.Trim(), CartItems);

            if (order == null)
            {
                ErrorMessage = "Medarbejdernummeret blev ikke fundet, eller kurven er tom. Ordren blev ikke oprettet.";
                return Page();
            }

            // Ordren er gemt, saa kurven skal toemmes for ikke at kunne bestilles igen
            _cartService.ClearCart();

            return RedirectToPage("/Confirmation", new { id = order.Id });
        }

        private void LoadCart()
        {
            CartItems = _cartService.GetCart();

            var products = _productService.GetAllProducts().ToDictionary(p => p.Id);
            Total = _priceService.CalculateCartTotal(CartItems, products);
        }
    }
}
