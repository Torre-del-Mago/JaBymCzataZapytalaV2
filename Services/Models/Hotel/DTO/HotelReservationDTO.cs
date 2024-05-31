namespace Models.Hotel.DTO
{
    public class HotelReservationDTO
    {
        public int HotelId { get; set; }

        public DateOnly BeginDate { get; set; }

        public DateOnly EndDate { get; set; }

        public List<RoomDTO> Rooms { get; set; }
        
        public int OfferId { get; set; }
    }
}
