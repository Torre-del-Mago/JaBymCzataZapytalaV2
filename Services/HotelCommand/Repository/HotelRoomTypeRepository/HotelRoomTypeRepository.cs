using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.HotelRoomTypeRepository
{
    public class HotelRoomTypeRepository : IHotelRoomTypeRepository
    {
        private readonly HotelContext _context;

        public HotelRoomTypeRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<HotelRoomType>> GetAllHotelRoomTypesAsync()
        {
            return await _context.HotelRoomTypes.ToListAsync();
        }

        public async Task<HotelRoomType> GetHotelRoomTypeByIdAsync(int hotelRoomTypeId)
        {
            return await _context.HotelRoomTypes.FindAsync(hotelRoomTypeId);
        }

        // Add other methods as needed
    }
}