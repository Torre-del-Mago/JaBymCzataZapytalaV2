using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.HotelRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllHotelsAsync();

        Task<Hotel> GetHotelByIdAsync(int hotelId);
    }
}
