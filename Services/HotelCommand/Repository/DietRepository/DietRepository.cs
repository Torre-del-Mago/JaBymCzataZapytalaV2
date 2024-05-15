using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HotelCommand.Repository.DietRepository
{
    public class DietRepository : IDietRepository
    {
        private readonly HotelContext _context;

        public DietRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Diet>> GetAllDietsAsync()
        {
            return await _context.Diets.ToListAsync();
        }

        public async Task<Diet> GetDietByIdAsync(int dietId)
        {
            return await _context.Diets.FindAsync(dietId);
        }

    }
}