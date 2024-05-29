using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.ReservedRoomRepository
{
    public class ReservedRoomRepository : IReservedRoomRepository
    {
        private readonly HotelContext _context;

        public ReservedRoomRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<ReservedRoom>> GetAllReservedRoomsAsync()
        {
            return await _context.ReservedRooms.ToListAsync();
        }

        public async Task<ReservedRoom> GetReservedRoomByIdAsync(int reservedRoomId)
        {
            return await _context.ReservedRooms.FindAsync(reservedRoomId);
        }

        public void InsertReservedRoom(ReservedRoom room)
        {
            _context.ReservedRooms.Add(room);
            _context.SaveChanges();
        }
    }
}