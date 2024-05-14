// weź dane dla wycieczek (kryteria hoteli)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class GetHotelDataForTripsEvent : EventModel
    {
        public CriteriaForHotels Criteria {  get; set; }
    }
}