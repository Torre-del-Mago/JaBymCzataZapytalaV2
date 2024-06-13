using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelCommand.Repository.HotelRepository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllHotelsAsync();

        Task<Hotel> GetHotelByIdAsync(int hotelId);

        EntityEntry<Hotel> UpdateHotel(Hotel hotel);
    }
}
