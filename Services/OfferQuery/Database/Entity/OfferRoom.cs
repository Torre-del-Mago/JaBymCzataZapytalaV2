using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfferQuery.Database.Entity;

public class OfferRoom
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("offerId")]
    public int OfferId { get; set; }

    [BsonElement("roomType")]
    public string RoomType { get; set; }

    [BsonElement("numberOfRooms")]
    public int NumberOfRooms { get; set; }
}