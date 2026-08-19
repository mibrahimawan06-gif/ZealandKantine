using Microsoft.EntityFrameworkCore;
using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    public class OrderRepo : IOrderRepository
    {
        private readonly ZealandDBContext _context;

        public OrderRepo(ZealandDBContext context)
        {
            _context = context;
        }

        // Ordre og ordrelinjer gemmes i én transaktion, fordi EF Core
        // sporer hele objektgrafen under Add()
        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        // Eager loading: uden Include ville hver ordrelinje og hver vare
        // koste et selvstaendigt databasekald (N+1-problemet)
        public Order? GetById(int id)
            => _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderLines)
                    .ThenInclude(l => l.Product)
                .FirstOrDefault(o => o.Id == id);

        public List<Order> GetAll()
            => _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderLines)
                    .ThenInclude(l => l.Product)
                .OrderByDescending(o => o.OrderTime)
                .ToList();
    }
}
