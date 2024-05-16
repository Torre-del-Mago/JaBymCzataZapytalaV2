using HotelQuery.Database.Entity;
using Models.Hotel.DTO;

namespace HotelQuery.Repository;

public interface IHotelRepository
{
    public List<Hotel> getHotels();
    public Hotel getHotel(int hotelId);
    
    public List<RoomType> getRoomTypes();
    public RoomType getRoomType(int roomTypeId);
    
    public List<Diet> getDiets();
    public Diet getDiet (int dietId);
    
    public List<Reservation> getReservations();
    public Reservation getReservation(int reservationId);
    public List<Reservation> getReservationWithin(DateTime from, DateTime to);
    
    public List<ReservedRoom> getReservedRooms();
    public ReservedRoom getReservedRoom(int reservedRoomId);
    public List<ReservedRoom> getRoomsForReservation(Guid reservationId);

}