using Microsoft.EntityFrameworkCore;
using ZealandKantine.Repo;
using ZealandKantine.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            // HttpOnly forhindrer JavaScript i at laese sessionscookien
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Kraeves af CartService og AuthService for at tilgaa sessionen
        // uden for en PageModel
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext<ZealandDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Repositories registreres via deres interface, saa implementationen
        // kan udskiftes uden aendringer i servicelaget
        builder.Services.AddScoped<IProductRepository, ProductRepo>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepo>();
        builder.Services.AddScoped<IOrderRepository, OrderRepo>();

        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<OrderService>();
        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<PriceService>();
        builder.Services.AddScoped<AuthService>();

        var app = builder.Build();

        // Opretter databasen med skema og startdata ved foerste koersel.
        // Ved skemaaendringer skal databasen slettes manuelt - se rapportens
        // afsnit om kendte begraensninger.
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ZealandDBContext>();
            db.Database.EnsureCreated();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSession();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
