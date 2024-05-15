using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class ReservedRoom
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("reservationId")]
    public Guid ReservationId { get; set; }
    
    [BsonElement("hotelRoomTypesId")]
    public Guid HotelRoomTypesId { get; set; }
    
    [BsonElement("numberOfRooms")]
    public int NumberOfRooms { get; set; }
}