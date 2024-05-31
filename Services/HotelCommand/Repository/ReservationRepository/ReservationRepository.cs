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
            return _context.Reservations.ToList();
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return  _context.Reservations.Find(reservationId);
        }

        public async Task<Reservation> GetReservationByOfferIdAsync(int offerId)
        {
            return _context.Reservations
                .Include(r => r.Rooms)
                .Include(r => r.Hotel)
                .FirstOrDefault(r => r.OfferId == offerId);
        }

        public async Task<List<Reservation>> GetReservationByHotelIdDatesAndNotDeleted(int hotelId, DateOnly beginDate, DateOnly endDate)
        {
            var reservations =  _context.Reservations
                .Where(r => r.HotelId == hotelId && r.From < endDate && r.To > beginDate)
                .ToList();

            var validReservations = new List<Reservation>();

            foreach (var reservation in reservations)
            {
                var deletedEvent =  _context.Events
                    .Any(e => e.ReservationId == reservation.Id && e.EventType == "DELETED");

                if (!deletedEvent)
                {
                    validReservations.Add(reservation);
                }
            }
            return validReservations;
        }

        public void InsertReservation(Reservation reservation)
        {
            Console.WriteLine("Key of Reservation1: " + reservation.Id);
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            Console.WriteLine("Key of Reservation2: " + reservation.Id);
        }

        // HERE YOU SHOULD ADD QUERY ABOUT RESERVATION_EVENT
    }
}