using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.FlightConnectionRepository;

public interface IFlightConnectionRepository
{
    Task<List<FlightConnection>> GetAllFlightConnectionsAsync();
    Task<FlightConnection> GetFlightConnectionByIdAsync(int flightConnectionId);
}