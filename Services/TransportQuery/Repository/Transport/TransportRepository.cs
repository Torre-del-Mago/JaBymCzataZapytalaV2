using MongoDB.Driver;
using TransportQuery.Database.Entity;

namespace TransportQuery.Repository.Transport
{
    public class TransportRepository : ITransportRepository
    {
        const string ConnectionString = "mongodb://root:example@mongo:27017/";
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }

        public TransportRepository()
        {
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase("transport_query");
        }
        
        public List<Database.Entity.Transport> GetTransportsById(int flightConnectionId)
        {
            var transportCollection = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
            return transportCollection.Where(t => t.Id == flightConnectionId).ToList();
        }
        
        public int GetNumberOfTakenSeatsForTransport(int transportId)
        {
            var ticketCollection = Database.GetCollection<ReservedTicket>("reserved_tickets").AsQueryable();

            int numberOfSeatsTaken = ticketCollection.Where(t => t.TransportId == transportId).Sum(t => t.NumberOfReservedSeats);
            return numberOfSeatsTaken;
        }
        
        public List<FlightConnection> GetDepartureFlightConnections(string departure)
        {
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => c.DepartureLocation == departure).ToList();
            return result;
        }
        
        public List<FlightConnection> GetArrivalFlightConnections(string arrival)
        {
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => c.ArrivalLocation == arrival).ToList();
            return result;
        }

        public List<FlightConnection> GetFlightList(string country)
        {
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => c.ArrivalCountry == country).ToList();
            return result;
        }
    }
}
