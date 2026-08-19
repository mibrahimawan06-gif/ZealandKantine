using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Order? GetById(int id);
        List<Order> GetAll();
    }
}
