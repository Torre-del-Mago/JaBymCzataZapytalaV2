using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class TransportAgencyChange
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("eventName")]
    public string EventName { get; set; }
    
    [BsonElement("idChanged")]
    public int IdChanged { get; set; }
    
    [BsonElement("change")]
    public double Change { get; set; }
}