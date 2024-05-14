// weź dane dla wycieczek (Kryteria transportów)
using Models.Transport.DTO;

namespace Models.Transport
{
    public class GetTransportDataForTripsEvent : EventModel
    {
        public CriteriaForTransports Criteria {  get; set; }
    }
}