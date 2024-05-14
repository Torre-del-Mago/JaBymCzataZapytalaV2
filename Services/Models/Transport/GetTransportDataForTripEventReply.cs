// przyjmij dane dla wycieczki (TransportDTO)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class GetTransportDataForTripEventReply : EventModel
    {
        public TransportDTO Transport {  get; set; }
    }
}