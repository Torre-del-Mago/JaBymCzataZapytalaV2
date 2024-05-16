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
        
        public List<Transport> GetTransportsById(int flightConnectionId)
        {
            var transportCollection = _database.GetCollection<Transport>("transports").AsQueryable();
            return transportCollection.Where(t => t.Id == flightConnectionId).ToList();
        }
        
        public int GetNumberOfTakenSeatsForTransport(int transportId)
        {
            var ticketCollection = _database.GetCollection<ReservedTicket>("reserved_tickets").AsQueryable();

            int numberOfSeatsTaken = ticketCollection.Where(t => t.TransportId == transportId).Sum(t => t.NumberOfReservedSeats);
            return numberOfSeatsTaken;
        }
        
        public List<FlightConnection> GetDepartureFlightConnections(string departure)
        {
            var flightConnectionCollection = _database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => c.DepartureLocation == departure).ToList();
            return result;
        }
        
        public List<FlightConnection> GetArrivalFlightConnections(string arrival)
        {
            var flightConnectionCollection = _database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => c.ArrivalLocation == arrival).ToList();
            return result;
        }
    }
}
