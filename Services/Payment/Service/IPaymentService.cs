using Models.Payment;

namespace Payment.Service
{
    public interface IPaymentService
    {
        bool CanOfferBePaidFor(DateTime stamp, int offerId);

        void InsertPayment(CheckPaymentEvent paymentEvent);
    }
}
