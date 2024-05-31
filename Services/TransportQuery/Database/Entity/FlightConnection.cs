using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportQuery.Database.Entity;

public class FlightConnection
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("departureLocation")]
    public string DepartureLocation { get; set; }
    
    [BsonElement("arrivalLocation")]
    public string ArrivalLocation { get; set; }

    [BsonElement("arrivalCountry")]
    public string ArrivalCountry { get; set; }

}