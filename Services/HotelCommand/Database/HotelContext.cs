using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Database
{
    public class HotelContext : DbContext
    {
        public DbSet<Diet> Diets { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<HotelDiet> HotelDiets { get; set; }

        public DbSet<HotelRoomType> HotelRoomTypes { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<ReservedRoom> ReservedRooms { get; set; }

        public DbSet<RoomType> RoomTypes { get; set; }

        private readonly IConfiguration configuration;

        public HotelContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbPath"));
    }
}
