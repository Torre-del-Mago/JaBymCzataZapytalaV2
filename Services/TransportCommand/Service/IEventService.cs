using Models.Transport.DTO;

namespace TransportCommand.Service
{
    public interface IEventService
    {
        Task<bool> ReserveTransport(TransportReservationDTO dto);

        Task CancelTransport(int offerId);

        int ChangeNumberOfSeats(int transportId, int numberOfSeats);
        double ChangePricePerSeat(int transportId, double priceChange);
    }
}
