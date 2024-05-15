using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfferQuery.Database.Entity;

public class Offer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }

    [BsonElement("departureTicketId")]
    public string DepartureTicketId { get; set; }

    [BsonElement("arrivalTicketId")]
    public string ArrivalTicketId { get; set; }

    [BsonElement("departureTransportId")]
    public string DepartureTransportId { get; set; }

    [BsonElement("arrivalTransportId")]
    public string ArrivalTransportId { get; set; }

    [BsonElement("dateFrom")]
    public DateTime DateFrom { get; set; }

    [BsonElement("dateTo")]
    public DateTime DateTo { get; set; }

    [BsonElement("hotelId")]
    public string HotelId { get; set; }

    [BsonElement("numberOfAdults")]
    public int NumberOfAdults { get; set; }

    [BsonElement("numberOfNewborns")]
    public int NumberOfNewborns { get; set; }

    [BsonElement("numberOfToddlers")]
    public int NumberOfToddlers { get; set; }

    [BsonElement("numberOfTeenagers")]
    public int NumberOfTeenagers { get; set; }

    [BsonElement("userLogin")]
    public string UserLogin { get; set; }

    [BsonElement("offerStatus")]
    public string OfferStatus { get; set; }
}