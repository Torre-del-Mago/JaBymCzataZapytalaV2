using HotelQuery.Database.Entity;
using MongoDB.Driver;

namespace HotelQuery.Repository.Hotel;

public class HotelRepository : IHotelRepository
{
    const string ConnectionString = "mongodb://root:student@student-swarm01.maas:27017/";
    private MongoClient Client { get; set; }
    private IMongoDatabase Database { get; set; }
    
    public HotelRepository()
    {
        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase("rsww_184543_hotel_query");
    }
    
    public List<Database.Entity.Hotel> GetHotels()
    {
        var connectionCollection = Database.GetCollection<Database.Entity.Hotel>("hotels").AsQueryable();
        var result = connectionCollection.ToList();
        return result;
    }

    public Database.Entity.Hotel GetHotel(int hotelId)
    {
        var hotelCollection = Database.GetCollection<Database.Entity.Hotel>("hotels");
        var hotel = hotelCollection.Find(h => h.Id == hotelId).FirstOrDefault();
        return hotel;
    }

    public List<RoomType> GetRoomTypes()
    {
        var roomTypeCollection = Database.GetCollection<RoomType>("room_types").AsQueryable();
        var result = roomTypeCollection.ToList();
        return result;
    }

    public RoomType GetRoomType(int roomTypeId)
    {
        var roomTypeCollection = Database.GetCollection<RoomType>("room_types");
        var roomType = roomTypeCollection.Find(rt => rt.Id == roomTypeId).FirstOrDefault();
        return roomType;
    }

    public List<Diet> GetDiets()
    {
        var dietCollection = Database.GetCollection<Diet>("diets").AsQueryable();
        var result = dietCollection.ToList();
        return result;
    }

    public Diet GetDiet(int dietId)
    {
        var dietCollection = Database.GetCollection<Diet>("diets");
        var diet = dietCollection.Find(d => d.Id == dietId).FirstOrDefault();
        return diet;
    }

    public List<Database.Entity.Reservation> GetReservations()
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations").AsQueryable();
        var result = reservationCollection.ToList();
        return result;
    }

    public Database.Entity.Reservation GetReservation(int reservationId)
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations");
        var reservation = reservationCollection.Find(r => r.Id == reservationId).FirstOrDefault();
        return reservation;
    }

    public List<Database.Entity.Reservation> GetReservationWithin(DateOnly from, DateOnly to)
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations");
        var reservations = reservationCollection.Find(r => r.From >= from && r.To <= to).ToList();
        return reservations;
    }

    public List<ReservedRoom> GetReservedRooms()
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms").AsQueryable();
        var result = reservedRoomCollection.ToList();
        return result;
    }

    public ReservedRoom GetReservedRoom(int reservedRoomId)
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms");
        var reservedRoom = reservedRoomCollection.Find(rr => rr.Id == reservedRoomId).FirstOrDefault();
        return reservedRoom;
    }

    public List<ReservedRoom> GetRoomsForReservation(int reservationId)
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms");
        var reservedRooms = reservedRoomCollection.Find(rr => rr.ReservationId == reservationId).ToList();
        return reservedRooms;
    }

    public List<HotelRoomType> GetHotelRoomTypes()
    {
        var hotelRoomTypeCollection = Database.GetCollection<HotelRoomType>("hotel_room_types").AsQueryable();
        var result = hotelRoomTypeCollection.ToList();
        return result;
    }

    public HotelRoomType GetHotelRoomType(int hotelRoomTypeId)
    {
        var hotelRoomTypeCollection = Database.GetCollection<HotelRoomType>("hotel_room_types");
        var hotelRoomType = hotelRoomTypeCollection.Find(rr => rr.Id == hotelRoomTypeId).FirstOrDefault();
        return hotelRoomType;
    }
}
