// SprawdźPłatność(ofertaID)
namespace Models.Payment
{
    public class CheckPaymentEvent : EventModel
    {
        public int OfferId { get; set; }

        public DateTime TimeForPayment { get; set; }
    }
}