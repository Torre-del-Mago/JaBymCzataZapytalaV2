using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class ReservedRoom
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("reservationId")]
    public int ReservationId { get; set; }
    
    [BsonElement("hotelRoomTypesId")]
    public int HotelRoomTypesId { get; set; }
    
    [BsonElement("numberOfRooms")]
    public int NumberOfRooms { get; set; }
}