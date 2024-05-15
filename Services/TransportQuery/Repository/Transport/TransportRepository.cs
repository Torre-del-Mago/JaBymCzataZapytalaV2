using MassTransit.Futures.Contracts;
using MongoDB.Driver;
using TransportQuery.Database.Entity;
using TransportQuery.DTO;

namespace TransportQuery.Repository.TransportRepository
{
    public class TransportRepository : ITransportRepository
    {
        const string connectionUri = "mongodb://mongo:27017";

        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }

        public TransportRepository()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("transport_query");
        }

        public List<ConnectionDTO> getConnectionComingFrom(string destinationCity)
        {
            var connectionCollection = _database.GetCollection<FlightConnection>("flight_connection").AsQueryable();

            var result = connectionCollection.Where(c => c.ArrivalLocation == destinationCity).Select(c => new ConnectionDTO() { LocationName = c.ArrivalLocation, Id = c.Id }).ToList();
            return result;
        }

        public List<ConnectionDTO> getConnectionGoingTo(string destinationCity)
        {
            var connectionCollection = _database.GetCollection<FlightConnection>("flight_connection").AsQueryable();

            var result = connectionCollection.Where(c => c.DepartureLocation == destinationCity).Select(c => new ConnectionDTO() { LocationName = c.DepartureLocation, Id = c.Id }).ToList();
            return result;
        }

        public List<TransportDTO> getTransportsForConnection(string destinationId)
        {
            var transportCollection = _database.GetCollection<Database.Entity.Transport>("transport").AsQueryable();
            var result = transportCollection.Where(t => destinationId == t.ConnectionId).Select(t => new TransportDTO() { Id= t.Id, NumberOfSeats = t.NumberOfSeats, Price = t.PricePerSeat }).ToList();
            return result;
        }

        public List<Transport> getTransportsByIds(string departureId, string returnId)
        {
            var transportCollection = _database.GetCollection<Database.Entity.Transport>("transport").AsQueryable();
            return transportCollection.Where(t => t.Id == departureId || t.Id == returnId).ToList();
        }

        public int getNumberOfTakenSeatsForTransport(string transportId)
        {
            var ticketCollection = _database.GetCollection<ReservedTicket>("reserved_ticket").AsQueryable();
            int numberOfSeatsTaken = ticketCollection.Where(t => t.TransportId == transportId).Sum(t => t.NumberOfReservedSeats);
            return numberOfSeatsTaken;
        }
    }
}
