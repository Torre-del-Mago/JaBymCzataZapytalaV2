using Models.Transport.DTO;

namespace TransportCommand.Service
{
    public interface IEventService
    {
        Task<bool> ReserveTransport(TransportReservationDTO dto);

        Task CancelTransport(int offerId);

        int ChangeNumberOfSeats(int transportId, int numberOfSeats);
        void ChangePricePerSeat(int transportId, double priceChange);
    }
}
