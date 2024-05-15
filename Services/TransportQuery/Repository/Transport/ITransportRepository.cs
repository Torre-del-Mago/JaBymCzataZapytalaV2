using TransportQuery.DTO;

namespace TransportQuery.Repository.Transport
{
    public interface ITransportRepository
    {
        public List<ConnectionDTO> getConnectionGoingTo(string destinationCity);
        public List<ConnectionDTO> getConnectionComingFrom(string destinationCity);

        public List<TransportDTO> getTransportsForConnections(string destinationIds);

        public int getNumberOfTakenSeatsForTransport(string transportId);
    }
}
