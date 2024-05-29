using TransportQuery.Database.Entity;

namespace TransportQuery.Repository.Transport
{
    public interface ITransportRepository
    {
        public int GetNumberOfTakenSeatsForTransport(int transportId);
        public List<Database.Entity.Transport> GetTransportsById(int flightConnectionId);
        public List<FlightConnection> GetDepartureFlightConnections(string departure);
        public List<FlightConnection> GetArrivalFlightConnections(string arrival);
        
    }
}
