
using Models.Payment;
using MongoDB.Driver;

namespace Payment.Repository
{
    public class PaymentRepository : IPaymentRepository
    {

        const string connectionUri = "mongodb://root:student@student-swarm01.maas:27017/";

        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }

        public PaymentRepository()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("payment");
        }

        public Database.Entity.Payment GetPaymentForOfferId(int offerId)
        {
            var collection = _database.GetCollection<Database.Entity.Payment>("payments").AsQueryable();
            return collection.Where(p => p.OfferId == offerId).First();
        }

        public void InsertPayment(CheckPaymentEvent paymentEvent)
        {
            var collection = _database.GetCollection<Database.Entity.Payment>("payments");
            var payment = new Database.Entity.Payment()
            {
                Id = collection.AsQueryable().Count() + 1,
                OfferId = paymentEvent.OfferId,
                StartTimeOfPayment = paymentEvent.TimeForPayment,
                CorrelationId = paymentEvent.CorrelationId
            };

            collection.InsertOne(payment);
        }
    }
}
