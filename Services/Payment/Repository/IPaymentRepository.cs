using Models.Payment;

namespace Payment.Repository
{
    public interface IPaymentRepository
    {
        public Database.Entity.Payment GetPaymentForOfferId(int offerId);

        public void InsertPayment(CheckPaymentEvent paymentEvent);
    }
}
