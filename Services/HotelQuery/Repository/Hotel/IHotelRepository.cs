using HotelQuery.Database.Entity;

namespace HotelQuery.Repository.Hotel;

public interface IHotelRepository
{
    public List<Database.Entity.Hotel> GetHotels();
    public Database.Entity.Hotel GetHotel(int hotelId);
    
    public List<RoomType> GetRoomTypes();
    public RoomType GetRoomType(int roomTypeId);
    
    public List<Diet> GetDiets();
    public Diet GetDiet (int dietId);
    
    public List<Reservation> GetReservations();
    public Reservation GetReservation(int reservationId);
    public List<Reservation> GetReservationWithin(DateTime from, DateTime to);
    
    public List<ReservedRoom> GetReservedRooms();
    public ReservedRoom GetReservedRoom(int reservedRoomId);
    public List<ReservedRoom> GetRoomsForReservation(int reservationId);

    public List<HotelRoomType> GetHotelRoomTypes();
    public HotelRoomType GetHotelRoomType (int hotelRoomTypeId);
}