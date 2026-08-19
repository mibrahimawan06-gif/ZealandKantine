using ZealandKantine.models;
using ZealandKantine.Repo;

namespace ZealandKantine.Service
{
    /// <summary>
    /// Forretningslogik for kantinens sortiment.
    /// </summary>
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public void AddProduct(string name, string? description, decimal price, bool isDrink)
        {
            _repo.Add(new Product
            {
                Name = name,
                Description = description,
                Price = price,
                IsDrink = isDrink
            });
        }

        /// <summary>
        /// Opdaterer en eksisterende vare. Varetypen kan ikke aendres, da
        /// den bestemmer rabatreglen og dermed ville aendre betydningen af
        /// varen paa tvaers af historiske ordrer.
        /// </summary>
        public bool UpdateProduct(int id, string name, string? description, decimal price)
        {
            var product = _repo.GetById(id);

            if (product == null) return false;

            product.Name = name;
            product.Description = description;
            product.Price = price;
            _repo.Update(product);

            return true;
        }

        public void DeleteProduct(int id) => _repo.Delete(id);

        public Product? GetProductById(int id) => _repo.GetById(id);

        public List<Product> GetAllProducts() => _repo.GetAll();

        public List<Product> GetFood() => _repo.GetFood();

        public List<Product> GetDrinks() => _repo.GetDrinks();
    }
}
