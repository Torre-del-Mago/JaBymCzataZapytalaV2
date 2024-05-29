using HotelCommand.Database;
using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservationEventRepository;

public class ReservationEventRepository : IReservationEventRepository
{
    private readonly HotelContext _context;
    private ReservationRepository.ReservationRepository _reservationRepository;

    public ReservationEventRepository(HotelContext context, ReservationRepository.ReservationRepository repository)
    {
        _context = context;
        _reservationRepository = repository;
    }
    
    public Task<List<ReservationEvent>> GetAllHotelEventsForReservationId(int reservationId)
    {
        return Task.FromResult(_context.Events.Where(e => e.ReservationId == reservationId).OrderBy(e => e.Id).ToList());
    }

    public async Task InsertReservationEvent(int reservationId)
    {
        var reservation = new ReservationEvent
        {
            EventType = EventType.Created,
            TimeStamp = DateTime.UtcNow,
            ReservationId = reservationId,
            Reservation = await _reservationRepository.GetReservationByIdAsync(reservationId)
        };
        _context.Events.Add(reservation);
    }

    public async Task InsertCancellationEvent(int offerId)
    {
        var reservationByOfferId = await _reservationRepository.GetReservationByOfferIdAsync(offerId);
        var reservation = new ReservationEvent
        {
            EventType = EventType.Deleted,
            TimeStamp = DateTime.UtcNow,
            ReservationId = reservationByOfferId.Id,
            Reservation = reservationByOfferId
        };
        _context.Events.Add(reservation);
    }
}