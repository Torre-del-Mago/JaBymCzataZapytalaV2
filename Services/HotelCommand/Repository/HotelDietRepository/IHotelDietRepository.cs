using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelCommand.Repository.HotelDietRepository
{
    public interface IHotelDietRepository
    {
        Task<List<HotelDiet>> GetAllHotelDietsAsync();

        Task<HotelDiet> GetHotelDietByIdAsync(int hotelDietId);

        EntityEntry<HotelDiet> AddHotelDiet(HotelDiet hotelDiet);
    }
}