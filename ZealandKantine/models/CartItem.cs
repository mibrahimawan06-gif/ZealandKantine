namespace ZealandKantine.models
{
    /// <summary>
    /// En linje i brugerens indkoebskurv. Lever kun i sessionen og gemmes
    /// aldrig i databasen. Prisen her er en visningskopi - den rigtige pris
    /// hentes altid fra databasen, naar ordren oprettes.
    /// </summary>
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDrink { get; set; }
    }
}
