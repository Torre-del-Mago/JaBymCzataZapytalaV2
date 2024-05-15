using HotelCommand.Database;

namespace HotelCommand.Repository.HotelRepository
{
    public class HotelRepository : IHotelRepository
    {
        private HotelContext _context;

        public HotelRepository(HotelContext context)
        {
            _context = context;
        }
    }
}
