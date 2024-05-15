using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.RoomTypeRepository
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAllRoomTypesAsync();
        Task<RoomType> GetRoomTypeByIdAsync(int roomTypeId);
    }
}