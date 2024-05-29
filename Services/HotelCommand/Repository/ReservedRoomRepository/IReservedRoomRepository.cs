using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservedRoomRepository
{
    public interface IReservedRoomRepository
    {
        Task<List<ReservedRoom>> GetAllReservedRoomsAsync();
        Task<ReservedRoom> GetReservedRoomByIdAsync(int reservedRoomId);
        void InsertReservedRoom(ReservedRoom room);
    }
}