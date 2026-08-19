namespace ZealandKantine.models
{
    /// <summary>
    /// Et gennemfoert koeb. Bindende ved oprettelse, jf. casen.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        // Fremmednoegle til Employee
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateTime OrderTime { get; set; }

        // Casen: maden er klar til afhentning 1 time efter bestilling
        public DateTime PickupTime { get; set; }

        // Samlet beloeb inkl. rabat, beregnet ved oprettelsen
        public decimal TotalAmount { get; set; }

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
