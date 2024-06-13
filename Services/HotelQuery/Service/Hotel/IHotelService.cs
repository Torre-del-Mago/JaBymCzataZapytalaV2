using Models.Admin.DTO;
using Models.Hotel.DTO;

namespace HotelQuery.Service.Hotel;

public interface IHotelService
{
    HotelsDTO GetHotelsForCriteria(CriteriaForHotels criteria);
    HotelDTO GetHotelForCriteria(CriteriaForHotel criteria);
    Task<bool> ReserveHotel(HotelReservationDTO dto);
    Task CancelHotel(int offerId);
    TopHotelsDTO GetTopHotels(int numberOfElements);
    TopRoomTypesDTO GetTopRoomTypes(int numberOfElements);
    void AddWatcher(int hotelId);
    void RemoveWatcher(int hotelId);
    bool IsSomeoneElseWatching(int hotelId);
    bool HasSomeoneReservedHotel(int hotelId);
    void AddDiet(int hotelId, int dietId);
    void ChangeHotelDiscount(int hotelId, double discountChange);
}