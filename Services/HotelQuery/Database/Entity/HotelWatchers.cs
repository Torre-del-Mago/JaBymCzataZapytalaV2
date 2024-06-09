using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class HotelWatchers
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("hotelId")]
    public int HotelId { get; set; }
    
    [BsonElement("count")]
    public int Count { get; set; }
}