// przyjmij dane dal wycieczek (transportyDTO)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class GetTransportDataForTripsEventReply : EventModel
    {
        public TransportsDTO Transports { get; set; }
    }
}