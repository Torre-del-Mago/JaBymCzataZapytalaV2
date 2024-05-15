using TransportQuery.Database.Entity;
using TransportQuery.Model;

namespace TransportQuery.Repository.TransportRepository
{
    public interface ITransportRepository
    {
        public List<ConnectionModel> getConnectionGoingTo(string destinationCity);
        public List<ConnectionModel> getConnectionComingFrom(string destinationCity);

        public List<TransportModel> getTransportsForConnection(string destinationIds);

        public int getNumberOfTakenSeatsForTransport(string transportId);
        public List<Transport> getTransportsByIds(string departureId, string returnId);
    }
}
