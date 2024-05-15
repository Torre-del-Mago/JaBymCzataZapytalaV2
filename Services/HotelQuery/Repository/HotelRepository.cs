using Models.Hotel;
using Models.Hotel.DTO;
using MongoDB.Driver;

namespace HotelQuery.Repository;

public class HotelRepository<TEntity>(IMongoDatabase database, string collectionName) : IHotelRepository<TEntity>
    where TEntity: class
{
    private readonly IMongoCollection<TEntity> _collection = database.GetCollection<TEntity>(collectionName);

    public async Task<TEntity> getHotelDataForTrip (CriteriaForHotel data)
    {
        
    }
    
    public async Task<TEntity> getHotelDataForTrips (CriteriaForHotels data)
    {

    }

    public async Task<TEntity> getHotelRooms(HotelDTO hotel)
    {
        
    }

    public async Task<TEntity> getRoomTypes(HotelDTO hotel)
    {
        
    }

    public async Task<TEntity> getDiets(HotelDTO hotel)
    {
        
    }
}