using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class HotelRoom
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("hotelId")]
    public Guid HotelId { get; set; }
    
    [BsonElement("numberOfPeople")]
    public int NumberOfPeople { get; set; }
    
    [BsonElement("city")]
    public string City { get; set; }
}