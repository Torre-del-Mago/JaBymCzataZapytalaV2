// weźDaneDlaWycieczki(kryteria hoteli)
using Models.Hotel.DTO;

namespace Models.Hotel
{
    public class GetHotelDataForTripEvent : EventModel
    {

        public CriteriaForHotel Criteria {  get; set; }

    }
}
