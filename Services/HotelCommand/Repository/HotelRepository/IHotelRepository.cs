using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.HotelRepository
{
    public interface IHotelRepository
    {
        public Task<List<Hotel>> GetAllHotelsAsync();

        public Task<Hotel> GetHotelByIdAsync(int hotelId);
    }
}
