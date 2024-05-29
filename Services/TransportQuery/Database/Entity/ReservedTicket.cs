using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportQuery.Database.Entity;

public class ReservedTicket
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("transportId")]
    public int TransportId { get; set; }
    
    [BsonElement("numberOfReservedSeats")]
    public int NumberOfReservedSeats { get; set; }
}