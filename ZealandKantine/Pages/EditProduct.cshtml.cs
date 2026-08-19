using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Ret en eksisterende vare. Varetypen kan ikke aendres, da den styrer
    /// rabatreglen. En prisaendring paavirker kun fremtidige bestillinger.
    /// </summary>
    public class EditProductModel : PageModel
    {
        private readonly ProductService _service;
        private readonly AuthService _authService;

        public EditProductModel(ProductService service,
            AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public IActionResult OnGet(int id)
        {
            if (!_authService.IsAdmin())
            {
                return RedirectToPage("/login");
            }

            var product = _service.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;
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

            var updated = _service.UpdateProduct(
                Product.Id, Product.Name, Product.Description, Product.Price);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToPage("/Index");
        }
    }
}
