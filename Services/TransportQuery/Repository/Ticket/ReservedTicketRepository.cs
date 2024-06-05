using Models.Hotel;
using MongoDB.Driver;
using TransportQuery.Database.Entity;
using TransportQuery.Repository.Transport;

namespace TransportQuery.Repository.Ticket
{
    public class ReservedTicketRepository : IReservedTicketRepository
    {
        const string ConnectionString = "mongodb://root:student@student-swarm01.maas:27017/";
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }

        private ITransportRepository TransportRepository { get; set; }

        public ReservedTicketRepository(ITransportRepository TransportRepository)
        {
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase("rsww_184543_transport_query");
            this.TransportRepository = TransportRepository;
        }

        public async Task AddTicketAsync(IClientSessionHandle session, ReservedTicket ticket)
        {
            var tickets = Database.GetCollection<Database.Entity.ReservedTicket>("reserved_tickets");
            await tickets.InsertOneAsync(session, ticket);
        }

        public async Task AddNewTicketStatusAsync(IClientSessionHandle session, ReservedTicketStatus status)
        {
            var statuses = Database.GetCollection<Database.Entity.ReservedTicketStatus>("reserved_ticket_statuses");
            await statuses.InsertOneAsync(session, status);
        }

        public async Task<bool> FindIfTicketsAreCanceledAsync(int OfferId)
        {
            var ticketStatus = Database.GetCollection<ReservedTicketStatus>("reserved_ticket_statuses");
            var filter = Builders<ReservedTicketStatus>.Filter.And(
                Builders<ReservedTicketStatus>.Filter.Eq(t => t.OfferId, OfferId),
                Builders<ReservedTicketStatus>.Filter.Eq(t => t.TicketStatus, "CANCELED")
            );
            return await ticketStatus.Find(filter).AnyAsync();
        }

        public async Task<bool> FindTicketStatusAsync(int OfferId)
        {
            var ticketStatus = Database.GetCollection<ReservedTicketStatus>("reserved_ticket_statuses");
            var filter = Builders<ReservedTicketStatus>.Filter.And(
                Builders<ReservedTicketStatus>.Filter.Eq(t => t.OfferId, OfferId)
            );
            return await ticketStatus.Find(filter).AnyAsync();
        }

        public async Task<bool> ReserveTicketsAsync(int ArrivalTransportId, int ReturnTransportId, int NumberOfPeople, int OfferId,
            int ArrivalTicketId, int ReturnTicketId)
        {
            using (var session = await Client.StartSessionAsync())
            {
                try
                {
                    if (FindIfTicketsAreCanceledAsync(OfferId).Result)
                    {
                        throw new Exception("Tickets are canceled.");
                    }

                    var transports = Database.GetCollection<Database.Entity.Transport>("transports").AsQueryable();
                    var arrivalTransport = transports.First(t => t.Id == ArrivalTransportId);
                    if (arrivalTransport.NumberOfSeats - TransportRepository.GetNumberOfTakenSeatsForTransport(ArrivalTransportId) < NumberOfPeople)
                    {
                        throw new Exception("Not enough seats available on arrival transport.");
                    }

                    var returnTransport = transports.First(t => t.Id == ReturnTransportId);
                    if (returnTransport.NumberOfSeats - TransportRepository.GetNumberOfTakenSeatsForTransport(ReturnTransportId) < NumberOfPeople)
                    {
                        throw new Exception("Not enough seats available on return transport.");
                    }

                    var newArrivalTicket = new ReservedTicket()
                    {
                        Id = ArrivalTicketId,
                        TransportId = ArrivalTransportId,
                        NumberOfReservedSeats = NumberOfPeople,
                        OfferId = OfferId
                    };
                    await AddTicketAsync(session, newArrivalTicket);

                    var newReturnTicket = new ReservedTicket()
                    {
                        Id = ReturnTicketId,
                        TransportId = ReturnTransportId,
                        NumberOfReservedSeats = NumberOfPeople,
                        OfferId = OfferId
                    };
                    await AddTicketAsync(session, newReturnTicket);

                    var newStatus = new ReservedTicketStatus()
                    {
                        Id = OfferId,
                        OfferId = OfferId,
                        TicketStatus = "RESERVED",
                    };
                    await AddNewTicketStatusAsync(session, newStatus);

                    return true;
                }
                catch (Exception e)
                {
                    Console.Error.Write(e);
                    return false;
                }
            }
        }

        public async Task CancelTickets(int OfferId)
        {
            using (var session = await Client.StartSessionAsync())
            {
                try
                {
                    var newStatus = new ReservedTicketStatus()
                    {
                        Id = OfferId,
                        OfferId = OfferId,
                        TicketStatus = "CANCELED",
                    };
                    if (FindTicketStatusAsync(OfferId).Result)
                    {
                        var tickets = Database.GetCollection<ReservedTicket>("reserved_tickets");
                        var ticketStatus = Database.GetCollection<ReservedTicketStatus>("reserved_ticket_statuses");
                        var filterStatus = Builders<ReservedTicketStatus>.Filter.And(
                            Builders<ReservedTicketStatus>.Filter.Eq(t => t.OfferId, OfferId)
                        );
                        await ticketStatus.FindOneAndReplaceAsync(session, filterStatus, newStatus);
                        var filterTickets = Builders<ReservedTicket>.Filter.And(
                            Builders<ReservedTicket>.Filter.Eq(t => t.OfferId, OfferId)
                        );

                        await tickets.DeleteManyAsync(session, filterTickets);
                    }
                    else
                    {
                        await AddNewTicketStatusAsync(session,newStatus);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.Write(e);
                }
            }
        }
    }
}
