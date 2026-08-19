namespace ZealandKantine.models
{
    /// <summary>
    /// En enkelt varelinje paa en ordre.
    /// UnitPrice er et snapshot af den pris, medarbejderen faktisk betalte.
    /// Prisen kopieres bevidst hertil i stedet for at pege paa Product.Price,
    /// saa en senere prisaendring ikke aendrer allerede gennemfoerte koeb.
    /// </summary>
    public class OrderLine
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        // Stykpris efter evt. medarbejderrabat, laast paa bestillingstidspunktet
        public decimal UnitPrice { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }
}
