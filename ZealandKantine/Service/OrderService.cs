using ZealandKantine.models;
using ZealandKantine.Repo;

namespace ZealandKantine.Service
{
    /// <summary>
    /// Forretningslogikken omkring et gennemfoert koeb.
    /// Det er her kurven bliver til en bindende ordre i databasen.
    /// </summary>
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IProductRepository _productRepo;
        private readonly PriceService _priceService;

        public OrderService(
            IOrderRepository orderRepo,
            IEmployeeRepository employeeRepo,
            IProductRepository productRepo,
            PriceService priceService)
        {
            _orderRepo = orderRepo;
            _employeeRepo = employeeRepo;
            _productRepo = productRepo;
            _priceService = priceService;
        }

        /// <summary>
        /// Opretter en ordre ud fra kurven og et medarbejdernummer.
        /// Returnerer null, hvis kurven er tom, eller medarbejderen ikke findes.
        /// </summary>
        public Order? CreateOrder(string employeeNumber, List<CartItem> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
            {
                return null;
            }

            var employee = _employeeRepo.GetByEmployeeNumber(employeeNumber);

            if (employee == null)
            {
                return null;
            }

            var now = DateTime.Now;

            var order = new Order
            {
                EmployeeId = employee.Id,
                OrderTime = now,
                // Casen: bestillingen er klar til afhentning efter 1 time
                PickupTime = now.AddHours(1)
            };

            foreach (var item in cartItems)
            {
                // Prisen hentes fra databasen, ikke fra kurven. Kurven ligger
                // i brugerens session og maa ikke kunne bestemme, hvad der betales.
                var product = _productRepo.GetById(item.ProductId);

                if (product == null || item.Quantity <= 0)
                {
                    continue;
                }

                order.OrderLines.Add(new OrderLine
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    // Snapshot: prisen laases fast paa bestillingstidspunktet
                    UnitPrice = _priceService.CalculateUnitPrice(product)
                });
            }

            if (order.OrderLines.Count == 0)
            {
                return null;
            }

            order.TotalAmount = order.OrderLines.Sum(l => l.UnitPrice * l.Quantity);

            _orderRepo.Add(order);

            return order;
        }

        public Order? GetOrder(int id) => _orderRepo.GetById(id);

        public List<Order> GetAllOrders() => _orderRepo.GetAll();
    }
}
