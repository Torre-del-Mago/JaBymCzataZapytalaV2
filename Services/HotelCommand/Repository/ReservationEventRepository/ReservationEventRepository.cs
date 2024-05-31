using HotelCommand.Database;
using HotelCommand.Database.Tables;
using HotelCommand.Repository.ReservationRepository;

namespace HotelCommand.Repository.ReservationEventRepository;

public class ReservationEventRepository : IReservationEventRepository
{
    private readonly HotelContext _context;
    private readonly IReservationRepository _reservationRepository;

    public ReservationEventRepository(HotelContext context, IReservationRepository reservationRepository)
    {
        _context = context;
        _reservationRepository = reservationRepository;
    }
    
    public Task<List<ReservationEvent>> GetAllHotelEventsForReservationId(int reservationId)
    {
        return Task.FromResult(_context.Events.Where(e => e.ReservationId == reservationId).OrderBy(e => e.Id).ToList());
    }

    public async Task InsertReservationEvent(int reservationId)
    {
        Console.WriteLine("InsertReservationEvent - START");
        var reservation = new ReservationEvent
        {
            EventType = EventType.Created,
            TimeStamp = DateTime.UtcNow,
            ReservationId = reservationId,
            Reservation = await _reservationRepository.GetReservationByIdAsync(reservationId)
        };
        _context.Events.Add(reservation);
        _context.SaveChanges();
        Console.WriteLine("InsertReservationEvent - END");
    }

    public async Task InsertCancellationEvent(int offerId)
    {
        Console.WriteLine("InsertCancellationEvent - START");
        var reservationByOfferId = await _reservationRepository.GetReservationByOfferIdAsync(offerId);
        var reservation = new ReservationEvent
        {
            EventType = EventType.Deleted,
            TimeStamp = DateTime.UtcNow,
            ReservationId = reservationByOfferId.Id,
            Reservation = reservationByOfferId
        };
        _context.Events.Add(reservation);
        _context.SaveChanges();
        Console.WriteLine("InsertCancellationEvent - END");
    }
}