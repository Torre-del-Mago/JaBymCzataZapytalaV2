using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.Database.Entity
{
    public class Payment
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("startTimeOfPayment")]
        public DateTime StartTimeOfPayment { get; set; }

        [BsonElement("offerId")]
        public int OfferId { get; set; }

        [BsonElement("offerCorrelationId")]
        public Guid CorrelationId {  get; set; }

    }
}
