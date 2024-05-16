
namespace Models.Gate.Payment.Response
{
    public class PayResponse
    {
        public enum State
        {
            PAID,
            REJECTED
        };

        public int OfferId { get; set; }

        public State Answer { get; set; }
    }
}
