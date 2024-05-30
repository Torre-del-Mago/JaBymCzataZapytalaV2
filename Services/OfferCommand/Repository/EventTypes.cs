namespace OfferCommand.Repository
{
    public class EventTypes
    {
        public static string Created { get { return "CREATED"; } }
        public static string Reserved { get { return "RESERVED"; } }
        public static string NotReserved { get { return "NOT_RESERVED"; } }
        public static string Paid { get { return "PAID"; } }
        public static string NotPaidInTime { get { return "NOT_PAID_IN_TIME"; } }
        public static string Removed { get { return "REMOVED"; } }
    }
}
