using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public EntityEntry<HotelDiet> AddHotelDiet(HotelDiet hotelDiet)
        {
            return _context.HotelDiets.Add(hotelDiet);
        }

        // Add other methods as needed
    }
}