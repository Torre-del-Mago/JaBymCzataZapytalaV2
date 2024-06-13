using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.TransportRepository;

public interface ITransportRepository
{
    public Task<List<Transport>> GetAllTransportsAsync();

    public Task<Transport> GetTransportByIdAsync(int transpotId);

    public Transport GetTransportById(int transpotId);

    public void UpdatePricePerSeat(int transportId, double priceChange);

    public void UpdateNumberOfSeats(int transportId, int numberOfSeats);
}