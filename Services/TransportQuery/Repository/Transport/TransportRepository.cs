using MassTransit.Futures.Contracts;
using MongoDB.Driver;
using TransportQuery.Database.Entity;
using TransportQuery.Model;

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

        public List<ConnectionModel> getConnectionFrom(string destinationCity)
        {
            var connectionCollection = _database.GetCollection<FlightConnection>("flight_connections").AsQueryable();

            var result = connectionCollection.Where(c => c.ArrivalLocation == destinationCity).Select(c => new ConnectionModel() { LocationName = c.ArrivalLocation, Id = c.Id }).ToList();
            return result;
        }

        public List<ConnectionModel> getConnectionTo(string destinationCity)
        {
            var connectionCollection = _database.GetCollection<FlightConnection>("flight_connections").AsQueryable();

            var result = connectionCollection.Where(c => c.DepartureLocation == destinationCity).Select(c => new ConnectionModel() { LocationName = c.DepartureLocation, Id = c.Id }).ToList();
            return result;
        }

        public List<TransportModel> getTransportsForConnection(int destinationId)
        {
            var transportCollection = _database.GetCollection<Transport>("transports").AsQueryable();
            var result = transportCollection.Where(t => destinationId == t.ConnectionId).Select(t => new TransportModel() { Id= t.Id, NumberOfSeats = t.NumberOfSeats, Price = t.PricePerSeat }).ToList();
            return result;
        }

        public List<Transport> getTransportsByIds(int departureId, int returnId)
        {
            var transportCollection = _database.GetCollection<Transport>("transports").AsQueryable();
            return transportCollection.Where(t => t.Id == departureId || t.Id == returnId).ToList();
        }

        public int getNumberOfTakenSeatsForTransport(int transportId)
        {
            var ticketCollection = _database.GetCollection<ReservedTicket>("reserved_tickets").AsQueryable();

            int numberOfSeatsTaken = ticketCollection.Where(t => t.TransportId == transportId).Sum(t => t.NumberOfReservedSeats);
            return numberOfSeatsTaken;
        }

        public List<Transport> GetTransports()
        {
            var transportCollection = _database.GetCollection<Transport>("transports").AsQueryable();
            var result = transportCollection.ToList();
            return result;
        }

        public Transport GetTransport(int transportId)
        {
            var transportCollection = _database.GetCollection<Transport>("transports");
            var hotelRoomType = transportCollection.Find(rr => rr.Id == transportId).FirstOrDefault();
            return hotelRoomType;
        }
    }
}
