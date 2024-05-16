using TransportQuery.Database.Entity;
using TransportQuery.Model;

namespace TransportQuery.Repository.TransportRepository
{
    public interface ITransportRepository
    {
        public int GetNumberOfTakenSeatsForTransport(int transportId);
        public List<Transport> GetTransportsById(int flightConnectionId);
        public List<FlightConnection> GetDepartureFlightConnections(string departure);
        public List<FlightConnection> GetArrivalFlightConnections(string arrival);
        
    }
}
