using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.HotelDietRepository
{
    public class HotelDietRepository : IHotelDietRepository
    {
        private readonly HotelContext _context;

        public HotelDietRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<HotelDiet>> GetAllHotelDietsAsync()
        {
            return await _context.HotelDiets.ToListAsync();
        }

        public async Task<HotelDiet> GetHotelDietByIdAsync(int hotelDietId)
        {
            return await _context.HotelDiets.FindAsync(hotelDietId);
        }

        // Add other methods as needed
    }
}