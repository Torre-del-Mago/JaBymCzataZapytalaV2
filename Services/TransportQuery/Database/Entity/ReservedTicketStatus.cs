using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportQuery.Database.Entity
{
    public class ReservedTicketStatus
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("ticketStatus")]
        public string TicketStatus { get; set; }

        [BsonElement("offerId")]
        public int OfferId { get; set; }
    }
}
