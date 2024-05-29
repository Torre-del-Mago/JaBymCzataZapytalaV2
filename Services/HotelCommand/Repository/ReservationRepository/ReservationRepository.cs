using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.ReservationRepository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelContext _context;

        public ReservationRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _context.Reservations.FindAsync(reservationId);
        }

        public async Task<Reservation> GetReservationByOfferIdAsync(int offerId)
        {
            return await _context.Reservations
                .Include(r => r.Rooms)
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.OfferId == offerId);
        }

        public async Task<List<Reservation>> GetReservationByHotelIdDatesAndNotDeleted(int hotelId, DateTime beginDate, DateTime endDate)
        {
            var reservations = await _context.Reservations
                .Where(r => r.HotelId == hotelId && r.From < endDate && r.To > beginDate)
                .ToListAsync();

            var validReservations = new List<Reservation>();

            foreach (var reservation in reservations)
            {
                var deletedEvent = await _context.Events
                    .AnyAsync(e => e.ReservationId == reservation.Id && e.EventType == "DELETED");

                if (!deletedEvent)
                {
                    validReservations.Add(reservation);
                }
            }
            return validReservations;
        }

        public void InsertReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        // HERE YOU SHOULD ADD QUERY ABOUT RESERVATION_EVENT
    }
}