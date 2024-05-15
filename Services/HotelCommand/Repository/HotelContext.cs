using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository
{
    public class HotelContext : DbContext
    {
        private readonly IConfiguration configuration;

        public HotelContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbPath"));
    }
}
