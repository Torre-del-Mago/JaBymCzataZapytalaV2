using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class ReservationStatus
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("reservationStatus")]
    public string reservationStatus { get; set; }

    [BsonElement("offerId")]
    public int OfferId { get; set; }
}