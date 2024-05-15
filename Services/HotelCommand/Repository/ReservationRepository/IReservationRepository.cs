using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservationRepository
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
    }
}