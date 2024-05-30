using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.DietRepository
{
    public interface IDietRepository
    {
        Task<List<Diet>> GetAllDietsAsync();

        Task<Diet> GetDietByIdAsync(int dietId);
    }
}
