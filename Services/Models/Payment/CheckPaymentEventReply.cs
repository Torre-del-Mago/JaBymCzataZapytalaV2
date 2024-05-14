// zapłacono (ofertaID)
// niezapłaconoWCzasie (ofertaId)

namespace Models.Payment
{
    public class CheckPaymentEventReply : EventModel
    {
        public int OfferId { get; set; }

        public enum State
        {
            PAID,
            NOT_PAID_IN_TIME
        };

        public State Answer { get; set; }
    }
}