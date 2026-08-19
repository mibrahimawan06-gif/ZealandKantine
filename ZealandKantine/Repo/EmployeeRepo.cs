using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    public class EmployeeRepo : IEmployeeRepository
    {
        private readonly ZealandDBContext _context;

        public EmployeeRepo(ZealandDBContext context)
        {
            _context = context;
        }

        public Employee? GetByEmployeeNumber(string employeeNumber)
            => _context.Employees.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);

        public List<Employee> GetAll() => _context.Employees.ToList();
    }
}
