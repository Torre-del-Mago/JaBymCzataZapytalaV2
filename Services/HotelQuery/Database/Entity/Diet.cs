using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class Diet
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
}