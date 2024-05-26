namespace Payment.Repository
{
    public interface IPaymentRepository
    {
        public Database.Entity.Payment getPaymentForOfferId(int offerId);

        public void insertPayment(DateTime stamp, int offerId);
    }
}
