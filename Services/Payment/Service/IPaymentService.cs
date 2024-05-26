namespace Payment.Service
{
    public interface IPaymentService
    {
        bool canOfferBePaidFor(DateTime stamp, int offerId);

        void insertPayment(DateTime stamp, int offerId);
    }
}
