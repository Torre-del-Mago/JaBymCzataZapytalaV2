using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class HotelRoomType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("count")]
    public int Count { get; set; }
    
    [BsonElement("pricePerNight")]
    public int PricePerNight { get; set; }
    
    [BsonElement("hotelId")]
    public int RoomTypeId { get; set; }
}