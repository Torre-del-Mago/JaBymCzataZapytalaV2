using TransportQuery.Database.Entity;
using TransportQuery.Model;

namespace TransportQuery.Repository.TransportRepository
{
    public interface ITransportRepository
    {
        public List<ConnectionModel> getConnectionTo(string destination);
        public List<ConnectionModel> getConnectionFrom(string country);

        public List<TransportModel> getTransportsForConnection(int destinationIds);

        public int getNumberOfTakenSeatsForTransport(int transportId);
        public List<Transport> getTransportsByIds(int departureId, int returnId);
        
        public List<Transport> GetTransports();
        public Transport GetTransport(int transportId);
    }
}
