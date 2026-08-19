using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    public interface IEmployeeRepository
    {
        Employee? GetByEmployeeNumber(string employeeNumber);
        List<Employee> GetAll();
    }
}
