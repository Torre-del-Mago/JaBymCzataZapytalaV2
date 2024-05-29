using Models.Hotel.DTO;

namespace HotelCommand.Service;

public interface IEventService
{
    public Task<bool> reserveHotel(HotelReservationDTO dto);

    public Task cancelHotel(int offerId);
}