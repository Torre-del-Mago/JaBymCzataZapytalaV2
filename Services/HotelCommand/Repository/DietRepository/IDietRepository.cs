using HotelCommand.Database.Tables;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelCommand.Repository.DietRepository
{
    public interface IDietRepository
    {
        Task<List<Diet>> GetAllDietsAsync();

        Task<Diet> GetDietByIdAsync(int dietId);

        EntityEntry<Diet> UpdateDiet(Diet diet);
    }
}
