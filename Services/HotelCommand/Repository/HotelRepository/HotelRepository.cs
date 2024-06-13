using HotelCommand.Database;
using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelCommand.Repository.HotelRepository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelContext _context;

        public HotelRepository(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public Hotel GetHotel(int hotelId)
        {
            return _context.Hotels.SingleOrDefault(h => h.Id == hotelId);
        }

        public async Task<Hotel> GetHotelByIdAsync(int hotelId)
        {
            return await _context.Hotels.FindAsync(hotelId);
        }

        public void UpdateDiscount(int hotelId, float discount)
        {
            var hotel = _context.Hotels.Find(hotelId);
            if(hotel == null)
            {
                return;
            }
            hotel.Discount += discount;
            _context.Hotels.Update(hotel);
        }

        public EntityEntry<Hotel> UpdateHotel(Hotel hotel)
        {
            return _context.Hotels.Add(hotel);
        }
    }
}