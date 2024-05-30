using TransportQuery.Database.Entity;

namespace TransportQuery.Repository.Transport
{
    public interface ITransportRepository
    {
        int GetNumberOfTakenSeatsForTransport(int transportId);
        List<Database.Entity.Transport> GetTransportsById(int flightConnectionId);
        List<FlightConnection> GetDepartureFlightConnections(string departure);
        List<FlightConnection> GetArrivalFlightConnections(string arrival);
        
    }
}
