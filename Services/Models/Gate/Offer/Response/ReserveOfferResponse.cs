namespace Models.Gate.Offer.Response
{
    public class ReserveOfferResponse
    {
        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }

        public int OfferId { get; set; }

        public string Error { get; set; }

        public int Registration { get; set; }
    }
}
