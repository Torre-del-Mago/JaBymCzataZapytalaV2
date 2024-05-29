using Models.Payment;

namespace Payment.Service
{
    public interface IPaymentService
    {
        bool canOfferBePaidFor(DateTime stamp, int offerId);

        void insertPayment(CheckPaymentEvent paymentEvent);
    }
}
