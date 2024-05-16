using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.FlightConnectionRepository;

public interface IFlightConnectionRepository
{
    public Task<List<FlightConnection>> GetAllFlightConnectionsAsync();
    public Task<FlightConnection> GetFlightConnectionByIdAsync(int flightConnectionId);
}