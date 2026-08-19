using ZealandKantine.models;

namespace ZealandKantine.Service
{
    /// <summary>
    /// Indeholder kantinens prisregler. Reglerne ligger her og ikke i
    /// modelklasserne, saa de kan aendres ét sted.
    /// </summary>
    public class PriceService
    {
        // Casen: medarbejderen faar 10% rabat paa alle indkoeb
        // eksklusive drikkevarer
        private const decimal FoodDiscountRate = 0.10m;

        /// <summary>
        /// Stykpris efter evt. medarbejderrabat. Afrundes til hele oerer,
        /// saa summen af linjerne altid svarer til det viste totalbeloeb.
        /// </summary>
        public decimal CalculateUnitPrice(Product product)
        {
            if (product.IsDrink)
            {
                return product.Price;
            }

            return Math.Round(product.Price * (1 - FoodDiscountRate), 2);
        }

        public decimal CalculateLineTotal(Product product, int quantity)
            => CalculateUnitPrice(product) * quantity;

        /// <summary>
        /// Total for hele kurven. Priserne hentes fra de medsendte varer,
        /// aldrig fra kurvens egne prisfelter.
        /// </summary>
        public decimal CalculateCartTotal(IEnumerable<CartItem> cartItems, IDictionary<int, Product> products)
        {
            decimal total = 0;

            foreach (var item in cartItems)
            {
                if (products.TryGetValue(item.ProductId, out var product))
                {
                    total += CalculateLineTotal(product, item.Quantity);
                }
            }

            return total;
        }
    }
}
