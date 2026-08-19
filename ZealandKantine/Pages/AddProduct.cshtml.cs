using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Opret en vare i sortimentet. Erstatter de tidligere sider Addfood
    /// og AddDrink, som var identiske paa naer varetypen.
    /// </summary>
    public class AddProductModel : PageModel
    {
        private readonly ProductService _service;
        private readonly AuthService _authService;

        public AddProductModel(ProductService service,
            AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public IActionResult OnGet(bool isDrink = false)
        {
            if (!_authService.IsAdmin())
            {
                return RedirectToPage("/login");
            }

            Product = new Product { IsDrink = isDrink };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!_authService.IsAdmin())
            {
                return RedirectToPage("/login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _service.AddProduct(Product.Name, Product.Description, Product.Price, Product.IsDrink);
            return RedirectToPage("/Index");
        }
    }
}
