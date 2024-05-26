using Models.Transport.DTO;

namespace TransportCommand.Service
{
    public interface IEventService
    {
        public Task<bool> reserveTransport(TransportReservationDTO dto);
    }
}
