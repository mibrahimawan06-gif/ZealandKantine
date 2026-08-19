using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZealandKantine.models;
using ZealandKantine.Service;

namespace ZealandKantine.Pages
{
    /// <summary>
    /// Kantinepersonalets overblik over alle gennemfoerte ordrer.
    /// </summary>
    public class OrdersModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly AuthService _authService;

        public OrdersModel(OrderService orderService,
            AuthService authService)
        {
            _orderService = orderService;
            _authService = authService;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public IActionResult OnGet()
        {
            // Kun kantinepersonale maa se andres bestillinger
            if (!_authService.IsAdmin())
            {
                return RedirectToPage("/login");
            }

            Orders = _orderService.GetAllOrders();
            return Page();
        }
    }
}
