using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservationEventRepository;

public interface IReservationEventRepository
{
    Task<List<ReservationEvent>> GetAllHotelEventsForReservationId(int reservationId);

    Task InsertReservationEvent(int reservationId);

    Task InsertCancellationEvent(int offerId);
}