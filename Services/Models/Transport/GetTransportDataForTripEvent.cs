// weź dane dla wycieczki (transport Kryteria)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class GetTransportDataForTripEvent : EventModel
    {
        public CriteriaForTransport Criteria {  get; set; }
    }
}