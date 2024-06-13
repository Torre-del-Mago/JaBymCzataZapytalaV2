using Models.Hotel.DTO;

namespace HotelCommand.Service;

public interface IEventService
{
    Task<bool> ReserveHotel(HotelReservationDTO dto);
    Task CancelHotel(int offerId);

    Task AddDiet(int hotelId, int dietId);
    void ChangeHotelDiscount(int hotelId, double discountChange);
}