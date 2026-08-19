using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    /// <summary>
    /// Abstraherer dataadgangen til sortimentet, saa servicelaget ikke
    /// afhaenger af EF Core.
    /// </summary>
    public interface IProductRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        Product? GetById(int id);
        List<Product> GetAll();
        List<Product> GetFood();
        List<Product> GetDrinks();
    }
}
