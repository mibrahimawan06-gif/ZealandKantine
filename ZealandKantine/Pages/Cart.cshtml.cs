using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    public class CartModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly PriceService _priceService;
        private readonly AuthService _authService;

        public CartModel(
            CartService cartService,
            ProductService productService,
            PriceService priceService,
            AuthService authService)
        {
            _cartService = cartService;
            _productService = productService;
            _priceService = priceService;
            _authService = authService;
        }

        /// <summary>
        /// Visningsmodel: kurvlinje beriget med den pris, der gaelder i
        /// databasen lige nu.
        /// </summary>
        public class CartLine
        {
            public CartItem Item { get; set; } = new CartItem();
            public decimal UnitPrice { get; set; }
            public decimal LineTotal { get; set; }
        }

        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public decimal Total { get; set; }

        public IActionResult OnGet()
        {
            if (!_authService.IsLoggedIn())
            {
                return RedirectToPage("/login");
            }

            LoadCart();
            return Page();
        }

        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToPage();
        }

        public IActionResult OnPostClearCart()
        {
            _cartService.ClearCart();
            return RedirectToPage();
        }

        private void LoadCart()
        {
            var cart = _cartService.GetCart();
            var products = _productService.GetAllProducts().ToDictionary(p => p.Id);

            foreach (var item in cart)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                {
                    // Varen er fjernet fra sortimentet, mens den laa i kurven
                    continue;
                }

                var unitPrice = _priceService.CalculateUnitPrice(product);

                Lines.Add(new CartLine
                {
                    Item = item,
                    UnitPrice = unitPrice,
                    LineTotal = unitPrice * item.Quantity
                });
            }

            Total = Lines.Sum(l => l.LineTotal);
        }
    }
}
