using HotelQuery.Database.Entity;

namespace HotelQuery.Repository.Hotel;

public interface IHotelRepository
{
    List<Database.Entity.Hotel> GetHotels();
    Database.Entity.Hotel GetHotel(int hotelId);
    
    List<RoomType> GetRoomTypes();
    public RoomType GetRoomType(int roomTypeId);
    
    List<Diet> GetDiets();
    Diet GetDiet (int dietId);
    
    List<Database.Entity.Reservation> GetReservations();
    Database.Entity.Reservation GetReservation(int reservationId);
    List<Database.Entity.Reservation> GetReservationWithin(DateTime from, DateTime to);
    
    List<ReservedRoom> GetReservedRooms();
    ReservedRoom GetReservedRoom(int reservedRoomId);
    List<ReservedRoom> GetRoomsForReservation(int reservationId);

    List<HotelRoomType> GetHotelRoomTypes();
    HotelRoomType GetHotelRoomType (int hotelRoomTypeId);
}