using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfferQuery.Database.Entity;

public class OfferRoom
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("offerId")]
    public string OfferId { get; set; }

    [BsonElement("roomType")]
    public string RoomType { get; set; }

    [BsonElement("numberOfRooms")]
    public int NumberOfRooms { get; set; }
}