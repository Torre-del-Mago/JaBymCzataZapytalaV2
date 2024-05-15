using HotelQuery.Database.Entity;
using MongoDB.Driver;

namespace HotelQuery.Repository;

public class HotelRepository : IHotelRepository
{
    const string connectionString = "mongodb://mongo:27017";
    private MongoClient _client { get; set; }
    private IMongoDatabase _database { get; set; }
    
    public HotelRepository()
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase("hotel_query");
    }
    
    public List<Hotel> getHotels()
    {
        var connectionCollection = _database.GetCollection<Hotel>("hotels").AsQueryable();
        var result = connectionCollection.ToList();
        return result;
    }

    public Hotel getHotel(int hotelId)
    {
        var hotelCollection = _database.GetCollection<Hotel>("hotels");
        var hotel = hotelCollection.Find(h => h.Id == hotelId).FirstOrDefault();
        return hotel;
    }

    public List<RoomType> getRoomTypes()
    {
        var roomTypeCollection = _database.GetCollection<RoomType>("roomTypes").AsQueryable();
        var result = roomTypeCollection.ToList();
        return result;
    }

    public RoomType getRoomType(int roomTypeId)
    {
        var roomTypeCollection = _database.GetCollection<RoomType>("roomTypes");
        var roomType = roomTypeCollection.Find(rt => rt.Id == roomTypeId).FirstOrDefault();
        return roomType;
    }

    public List<Diet> getDiets()
    {
        var dietCollection = _database.GetCollection<Diet>("diets").AsQueryable();
        var result = dietCollection.ToList();
        return result;
    }

    public Diet getDiet(int dietId)
    {
        var dietCollection = _database.GetCollection<Diet>("diets");
        var diet = dietCollection.Find(d => d.Id == dietId).FirstOrDefault();
        return diet;
    }

    public List<Reservation> getReservations()
    {
        var reservationCollection = _database.GetCollection<Reservation>("reservations").AsQueryable();
        var result = reservationCollection.ToList();
        return result;
    }

    public Reservation getReservation(int reservationId)
    {
        var reservationCollection = _database.GetCollection<Reservation>("reservations");
        var reservation = reservationCollection.Find(r => r.Id == reservationId).FirstOrDefault();
        return reservation;
    }

    public List<Reservation> getReservationWithin(DateTime from, DateTime to)
    {
        var reservationCollection = _database.GetCollection<Reservation>("reservations");
        var reservations = reservationCollection.Find(r => r.From >= from && r.To <= to).ToList();
        return reservations;
    }

    public List<ReservedRoom> getReservedRooms()
    {
        var reservedRoomCollection = _database.GetCollection<ReservedRoom>("reservedRooms").AsQueryable();
        var result = reservedRoomCollection.ToList();
        return result;
    }

    public ReservedRoom getReservedRoom(int reservedRoomId)
    {
        var reservedRoomCollection = _database.GetCollection<ReservedRoom>("reservedRooms");
        var reservedRoom = reservedRoomCollection.Find(rr => rr.Id == reservedRoomId).FirstOrDefault();
        return reservedRoom;
    }

    public List<ReservedRoom> getRoomsForReservation(Guid reservationId)
    {
        var reservedRoomCollection = _database.GetCollection<ReservedRoom>("reservedRooms");
        var reservedRooms = reservedRoomCollection.Find(rr => rr.ReservationId == reservationId).ToList();
        return reservedRooms;
    }
}
