using Models.Hotel.DTO;

namespace HotelQuery.Repository.Reservation;

public interface IReservationRepository
{
    Task<bool> ReserveAsync(int HotelId, DateTime BeginDate, DateTime EndDate, List<RoomDTO> Rooms, int OfferId);
    Task CancelReservation(int OfferId);
}