using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.HotelDietRepository
{
    public interface IHotelDietRepository
    {
        Task<List<HotelDiet>> GetAllHotelDietsAsync();

        Task<HotelDiet> GetHotelDietByIdAsync(int hotelDietId);

    }
}