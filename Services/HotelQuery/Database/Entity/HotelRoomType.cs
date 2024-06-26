﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class HotelRoomType
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("count")]
    public int Count { get; set; }
    
    [BsonElement("pricePerNight")]
    public int PricePerNight { get; set; }
    
    [BsonElement("roomTypeId")]
    public int RoomTypeId { get; set; }
}