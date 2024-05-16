using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.TransportRepository;

public interface ITransportRepository
{
    public Task<List<Transport>> GetAllTransportsAsync();

    public Task<Transport> GetTransportByIdAsync(int transpotId);
}