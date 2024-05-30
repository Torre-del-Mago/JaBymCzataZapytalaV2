using Models.Hotel.DTO;

namespace HotelQuery.Service.Hotel;

public interface IHotelService
{
    HotelsDTO GetHotelsForCriteria(CriteriaForHotels criteria);
    HotelDTO GetHotelForCriteria(CriteriaForHotel criteria);
    Task<bool> ReserveHotel(HotelReservationDTO dto);
    Task CancelHotel(int offerId);
}