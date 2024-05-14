// Płątność się nie powodła (oferta id)
// zapłacono za ofertę (oferta id)

namespace Models.Payment
{
    public class PayEventReply : EventModel
    {
        public int OfferId { get; set; }

        public enum State
        {
            PAID,
            REJECTED
        };

        public State Answer { get; set; }

    }
}