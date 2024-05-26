
using MongoDB.Driver;

namespace Payment.Repository
{
    public class PaymentRepository : IPaymentRepository
    {

        const string connectionUri = "mongodb://mongo:27017";

        private MongoClient _client { get; set; }
        private IMongoDatabase _database { get; set; }

        PaymentRepository()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("payment");
        }

        public Database.Entity.Payment getPaymentForOfferId(int offerId)
        {
            var collection = _database.GetCollection<Database.Entity.Payment>("payments").AsQueryable();
            return collection.Where(p => p.OfferId == offerId).First();
        }

        public void insertPayment(DateTime stamp, int offerId)
        {
            var payment = new Database.Entity.Payment()
            {
                OfferId = offerId,
                StartTimeOfPayment = stamp
            };

            var collection = _database.GetCollection<Database.Entity.Payment>("payments");
            collection.InsertOne(payment);
        }
    }
}
