using System.ComponentModel.DataAnnotations;

namespace ZealandKantine.models
{
    /// <summary>
    /// En medarbejder paa Zealand. Koeb registreres paa medarbejdernummeret.
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        // Navigationsegenskab: en medarbejder kan have mange ordrer
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
