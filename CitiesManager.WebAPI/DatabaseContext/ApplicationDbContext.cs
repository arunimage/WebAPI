using CitiesManager.WebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public ApplicationDbContext()
    {
    }

    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<City>().HasData(
        new City
        {
          CityID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
          CityName = "Hyderabad"
        },
        new City
        {
          CityID = Guid.Parse("22222222-2222-2222-2222-222222222222"),
          CityName = "Bangalore"
        }
        );
    }
  }
}
