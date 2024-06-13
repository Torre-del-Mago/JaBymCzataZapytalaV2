using Models.Admin.DTO;
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
            return transportCollection.Where(t => t.ConnectionId == flightConnectionId).ToList();
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
        public List<FlightConnection> GetConnectionsForArrivalLocations(List<string> arrivalLocations)
        {
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => arrivalLocations.Contains(c.ArrivalLocation)).ToList();
            return result;
        }

        public List<FlightConnection> GetConnectionsForDepartureLocations(List<string> departureLocations)
        {
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result = flightConnectionCollection.Where(c => departureLocations.Contains(c.DepartureLocation)).ToList();
            return result;
        }

        public List<EntryDTO> GetTopDepartures(int numberOfElements)
        {
            var ticketCollection = Database.GetCollection<ReservedTicket>("reserved_tickets").AsQueryable();
            var transportCollection = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result =  from ticket in ticketCollection
                join transport in transportCollection on ticket.TransportId equals transport.Id
                join flightConnection in flightConnectionCollection on transport.ConnectionId equals flightConnection.Id
                where !"polska".Equals(flightConnection.ArrivalCountry)
                group flightConnection by flightConnection.DepartureLocation
                into grp
                select new EntryDTO() { Name = grp.Key, NumberOfElements = grp.Count() };
            return result.ToList();
        }

        public List<EntryDTO> GetTopDestinations(int numberOfElements)
        {       
            var ticketCollection = Database.GetCollection<ReservedTicket>("reserved_tickets").AsQueryable();
            var transportCollection = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
            var flightConnectionCollection = Database.GetCollection<FlightConnection>("flight_connections").AsQueryable();
            var result =  from ticket in ticketCollection
                join transport in transportCollection on ticket.TransportId equals transport.Id
                join flightConnection in flightConnectionCollection on transport.ConnectionId equals flightConnection.Id
                where !"polska".Equals(flightConnection.ArrivalCountry)
                group flightConnection by flightConnection.ArrivalCountry + " " + flightConnection.ArrivalLocation
                into grp
                select new EntryDTO() { Name = grp.Key, NumberOfElements = grp.Count() };
            return result.ToList();
        }

        public void UpdateNumberOfSeats(int transportId, int numberOfSeats)
        {
            var transportCollection = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
            Database.Entity.Transport transport = transportCollection.Where(t => t.Id == transportId).First();
            transport.NumberOfSeats += numberOfSeats;
            Database.GetCollection<Database.Entity.Transport>("transports")
                    .FindOneAndReplace(t => t.Id == transportId, transport);
        }

        public void UpdatePricePerSeat(int transportId, double pricePerSeat)
        {
            var transportCollection = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
            Database.Entity.Transport transport = transportCollection.Where(t => t.Id == transportId).First();
            transport.PricePerSeat += Convert.ToSingle(pricePerSeat);
            Database.GetCollection<Database.Entity.Transport>("transports")
                    .FindOneAndReplace(t => t.Id == transportId, transport);
        }
    }
}
