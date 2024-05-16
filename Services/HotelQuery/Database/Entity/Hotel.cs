using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("discount")]
    public float Discount { get; set; }
    
    [BsonElement("city")]
    public string City { get; set; }
    
    [BsonElement("country")]
    public string Country { get; set; }
    
    [BsonElement("diets")]
    public List<Diet> Diets { get; set; }
    
    [BsonElement("rooms")]
    public List<HotelRoomType> Rooms { get; set; }
}