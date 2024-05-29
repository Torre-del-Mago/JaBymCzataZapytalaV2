// zapłać (oferta id, kwota)

namespace Models.Payment
{
    public class PayEvent : EventModel
    {
        public int OfferId { get; set; }

        public int Amount { get; set; }

        public DateTime PaymentDateTime { get; set; }
    }
}