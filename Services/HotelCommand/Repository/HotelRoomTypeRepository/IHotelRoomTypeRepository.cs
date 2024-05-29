using HotelCommand.Database.Tables;
namespace HotelCommand.Repository.HotelRoomTypeRepository
{
    public interface IHotelRoomTypeRepository
    {
        Task<List<HotelRoomType>> GetAllHotelRoomTypesAsync();

        Task<HotelRoomType> GetHotelRoomTypeByIdAsync(int hotelRoomTypeId);
        
        Task<HotelRoomType> GetHotelRoomTypeByNameAsync(string hotelRoomTypeName);
    }
}