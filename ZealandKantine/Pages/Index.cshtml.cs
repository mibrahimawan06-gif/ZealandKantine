using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Kantinens menu. Al kurv-, pris- og adgangslogik ligger i services,
    /// saa denne PageModel kun haandterer selve siden.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly PriceService _priceService;
        private readonly AuthService _authService;

        public IndexModel(
            ProductService productService,
            CartService cartService,
            PriceService priceService,
            AuthService authService)
        {
            _productService = productService;
            _cartService = cartService;
            _priceService = priceService;
            _authService = authService;
        }

        public List<Product> FoodItems { get; set; } = new List<Product>();
        public List<Product> DrinkItems { get; set; } = new List<Product>();

        public bool IsAdmin => _authService.IsAdmin();
        public string? Username => HttpContext.Session.GetString("Username");
        public string? UserRole => HttpContext.Session.GetString("UserRole");

        [TempData]
        public string? Message { get; set; }

        public IActionResult OnGet()
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            LoadProducts();
            return Page();
        }

        public IActionResult OnPostAddToCart(int id)
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _cartService.AddToCart(product);
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostDeleteProduct(int id)
        {
            if (!_authService.IsAdmin())
            {
                return RedirectToPage("/login");
            }

            try
            {
                _productService.DeleteProduct(id);
            }
            catch (DbUpdateException)
            {
                // Databasen afviser sletning af varer, der indgaar i en
                // gennemfoert ordre, saa historikken forbliver komplet
                Message = "Varen kan ikke slettes, fordi den indgår i tidligere bestillinger.";
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostLogout()
        {
            _authService.Logout();
            return RedirectToPage("/login");
        }

        public int GetCartItemCount() => _cartService.GetItemCount();

        public decimal GetDiscountedPrice(Product product) => _priceService.CalculateUnitPrice(product);

        private void LoadProducts()
        {
            FoodItems = _productService.GetFood();
            DrinkItems = _productService.GetDrinks();
        }
    }
}
