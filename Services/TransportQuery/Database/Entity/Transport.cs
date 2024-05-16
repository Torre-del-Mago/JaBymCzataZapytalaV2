using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportQuery.Database.Entity;

public class Transport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("connectionId")]
    public int ConnectionId { get; set; }

    [BsonElement("numberOfSeats")]
    public int NumberOfSeats { get; set; }

    [BsonElement("departureDate")]
    public DateTime DepartureDate { get; set; }

    [BsonElement("pricePerSeat")]
    public float PricePerSeat { get; set; }
}