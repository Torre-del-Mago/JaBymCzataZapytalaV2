using TransportQuery.Database.Entity;
using TransportQuery.DTO;

namespace TransportQuery.Repository.TransportRepository
{
    public interface ITransportRepository
    {
        public List<ConnectionDTO> getConnectionGoingTo(string destinationCity);
        public List<ConnectionDTO> getConnectionComingFrom(string destinationCity);

        public List<TransportDTO> getTransportsForConnection(string destinationIds);

        public int getNumberOfTakenSeatsForTransport(string transportId);
        public List<Transport> getTransportsByIds(string departureId, string returnId);
    }
}
