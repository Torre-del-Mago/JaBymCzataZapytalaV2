using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class RoomType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("numberOfPeople")]
    public int NumberOfPeople { get; set; }
}