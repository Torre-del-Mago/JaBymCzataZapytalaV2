using Models.Hotel.DTO;

namespace HotelQuery.Repository.Reservation;

public interface IReservationRepository
{
    Task<bool> ReserveAsync(int HotelId, DateOnly BeginDate, DateOnly EndDate, List<RoomDTO> Rooms, int OfferId);
    Task CancelReservation(int OfferId);
}