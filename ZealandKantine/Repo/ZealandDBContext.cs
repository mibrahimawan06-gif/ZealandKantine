using Microsoft.EntityFrameworkCore;
using ZealandKantine.models;

namespace ZealandKantine.Repo
{
    /// <summary>
    /// EF Core DbContext. Kortlaegger domaeneklasserne til tabeller i SQL Server.
    /// </summary>
    public class ZealandDBContext : DbContext
    {
        public ZealandDBContext(DbContextOptions<ZealandDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Uden eksplicit praecision afrunder SQL Server decimalvaerdier
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OrderLine>().Property(l => l.UnitPrice).HasPrecision(18, 2);

            // Medarbejdernummeret skal vaere unikt - det er noeglen brugeren taster ind
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeNumber)
                .IsUnique();

            // En ordre uden linjer giver ingen mening, saa linjerne slettes med ordren
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderLines)
                .WithOne(l => l.Order)
                .HasForeignKey(l => l.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // En vare maa ikke kunne slettes, hvis den indgaar i en historisk ordre
            modelBuilder.Entity<OrderLine>()
                .HasOne(l => l.Product)
                .WithMany()
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Startdata, saa systemet kan demonstreres uden manuel indtastning
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, EmployeeNumber = "1001", Name = "Anne Jensen" },
                new Employee { Id = 2, EmployeeNumber = "1002", Name = "Bo Kristensen" },
                new Employee { Id = 3, EmployeeNumber = "1003", Name = "Camilla Dahl" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Dagens smoerrebroed", Description = "Tre stykker med dagens paalaeg", Price = 55.00m, IsDrink = false },
                new Product { Id = 2, Name = "Frikadeller med kartoffelsalat", Description = "Hjemmelavede frikadeller", Price = 65.00m, IsDrink = false },
                new Product { Id = 3, Name = "Cola 0,5 l", Description = "Kold sodavand", Price = 20.00m, IsDrink = true },
                new Product { Id = 4, Name = "Kildevand 0,5 l", Description = "Uden brus", Price = 12.00m, IsDrink = true }
            );
        }
    }
}
