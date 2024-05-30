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
        
        public DbSet<ReservationEvent> Events { get; set; }

       public HotelContext(DbContextOptions<HotelContext> options) : base(options) {}
       
       public HotelContext() : base() {}

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<Diet>().ToTable("Diet");
           modelBuilder.Entity<Hotel>().ToTable("Hotel");
           modelBuilder.Entity<HotelDiet>().ToTable("HotelDiet");
           modelBuilder.Entity<HotelRoomType>().ToTable("HotelRoomType");
           modelBuilder.Entity<Reservation>().ToTable("Reservation");
           modelBuilder.Entity<ReservedRoom>().ToTable("ReservedRoom");
           modelBuilder.Entity<RoomType>().ToTable("RoomType");
           modelBuilder.Entity<ReservationEvent>().ToTable("ReservationEvent");
       }
    }
}
