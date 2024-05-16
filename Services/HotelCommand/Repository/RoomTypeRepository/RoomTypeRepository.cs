using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.RoomTypeRepository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly HotelContext _context;

        public RoomTypeRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task<RoomType> GetRoomTypeByIdAsync(int roomTypeId)
        {
            return await _context.RoomTypes.FindAsync(roomTypeId);
        }

    }
}