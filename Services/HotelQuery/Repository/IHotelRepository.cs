using Models.Hotel.DTO;

namespace HotelQuery.Repository;

public interface IHotelRepository<TEntity> where TEntity: class
{
    Task<TEntity> getHotelDataForTrip(CriteriaForHotel criteria);
    Task<TEntity> getHotelDataForTrips(CriteriaForHotels criteria);
}