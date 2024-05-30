namespace Models.Hotel.DTO
{
    public class HotelReservationDTO
    {
        public int HotelId { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<RoomDTO> Rooms { get; set; }
        
        public int OfferId { get; set; }
    }
}
