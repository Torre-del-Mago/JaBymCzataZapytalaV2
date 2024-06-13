namespace Models.TravelAgency
{
    public class AddDietSyncEvent : EventModel
    {
        public int HotelId { get; set; }
        public int DietId { get; set; }
    }
}