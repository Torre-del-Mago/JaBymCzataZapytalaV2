using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportQuery.Database.Entity;

public class FlightConnection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("departureLocation")]
    public string DepartureLocation { get; set; }
    
    [BsonElement("arrivalLocation")]
    public string ArrivalLocation { get; set; }

    [BsonElement("country")]
    public string Country { get; set; }

}