using System.Text.Json;
using ZealandKantine.models;

namespace ZealandKantine.Service
{
    /// <summary>
    /// Haandterer indkoebskurven, som gemmes i serverens session som JSON.
    /// Samlet ét sted, saa PageModels ikke selv skal kende sessionsnoegler
    /// eller serialisering.
    /// </summary>
    public class CartService
    {
        private const string SessionKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public List<CartItem> GetCart()
        {
            var json = Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
            => Session.SetString(SessionKey, JsonSerializer.Serialize(cart));

        public void AddToCart(Product product)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == product.Id);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    IsDrink = product.IsDrink
                });
            }

            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item == null) return;

            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void ClearCart() => SaveCart(new List<CartItem>());

        public int GetItemCount() => GetCart().Sum(c => c.Quantity);
    }
}
