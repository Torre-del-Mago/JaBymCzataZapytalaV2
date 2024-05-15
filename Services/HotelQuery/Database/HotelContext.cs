using HotelQuery.Database.Entity;
using HotelQuery.Repository;
using MongoDB.Driver;

namespace HotelQuery.Database;

public class HotelContext(IMongoDatabase database)
{
    public IHotelRepository<Diet> Diets => new HotelRepository<Diet>(database, "diets");
    public IHotelRepository<Hotel> Hotels => new HotelRepository<Hotel>(database, "hotels");
    public IHotelRepository<Reservation> Reservations => new HotelRepository<Reservation>(database, "reservations");
    public IHotelRepository<ReservedRoom> ReservedRooms => new HotelRepository<ReservedRoom>(database, "reservedRooms");
    public IHotelRepository<RoomType> RoomTypes => new HotelRepository<RoomType>(database, "roomTypes");
}