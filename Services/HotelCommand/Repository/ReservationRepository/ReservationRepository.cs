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


        // HERE YOU SHOULD ADD QUERY ABOUT RESERVATION_EVENT
    }
}