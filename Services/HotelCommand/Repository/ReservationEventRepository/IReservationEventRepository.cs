using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservationEventRepository;

public interface IReservationEventRepository
{
    public Task<List<ReservationEvent>> GetAllHotelEventsForReservationId(int reservationId);

    public Task InsertReservationEvent(int reservationId);

    public Task InsertCancellationEvent(int offerId);
}