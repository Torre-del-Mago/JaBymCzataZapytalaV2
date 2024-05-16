using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.DietRepository
{
    public interface IDietRepository
    {
        public Task<List<Diet>> GetAllDietsAsync();

        public Task<Diet> GetDietByIdAsync(int dietId);
    }
}
