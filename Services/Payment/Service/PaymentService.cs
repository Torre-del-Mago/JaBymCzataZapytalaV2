
using Models.Payment;
using Payment.Repository;

namespace Payment.Service
{
    public class PaymentService : IPaymentService
    {
        private IPaymentRepository _repository;
        public PaymentService(IPaymentRepository repository) {
            _repository = repository;
        }
        public bool canOfferBePaidFor(DateTime stamp, int offerId)
        {
            var payment = _repository.GetPaymentForOfferId(offerId);
            if(payment == null)
            {
                return false;
            }

            return (stamp - payment.StartTimeOfPayment).TotalSeconds <= 60;
        }

        public void insertPayment(CheckPaymentEvent paymentEvent)
        {
            _repository.InsertPayment(paymentEvent);
        }


    }
}
