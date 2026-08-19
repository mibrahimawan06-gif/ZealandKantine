using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    public class ProductRepo : IProductRepository
    {
        private readonly ZealandDBContext _context;

        public ProductRepo(ZealandDBContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public Product? GetById(int id) => _context.Products.Find(id);

        public List<Product> GetAll() => _context.Products.ToList();

        public List<Product> GetFood() => _context.Products.Where(p => !p.IsDrink).ToList();

        public List<Product> GetDrinks() => _context.Products.Where(p => p.IsDrink).ToList();
    }
}
