using Models.Hotel.DTO;

namespace HotelQuery.Service.Hotel;

public interface IHotelService
{
    public HotelsDTO GetHotelsForCriteria(CriteriaForHotels criteria);
    
    public HotelDTO GetHotelForCriteria(CriteriaForHotel criteria);
}