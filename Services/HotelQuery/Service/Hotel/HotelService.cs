using HotelQuery.Database.Entity;
using HotelQuery.Repository.Hotel;
using HotelQuery.Repository.Reservation;
using Models.Admin.DTO;
using Models.Hotel.DTO;

namespace HotelQuery.Service.Hotel;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IReservationRepository _reservationRepository;

    public HotelService(IHotelRepository repository, IReservationRepository reservationRepository)
    {
        _hotelRepository = repository;
        _reservationRepository = reservationRepository;
    }
    
    public HotelsDTO GetHotelsForCriteria(CriteriaForHotels criteria)
    {
        var hotels = _hotelRepository.GetHotels();

        var filteredHotels = hotels.Where(h =>
            h.Country == criteria.Country &&
            IsHotelAvailable(h, criteria.BeginDate, criteria.EndDate, criteria.NumberOfPeople)).ToList();

        var hotelsDto = filteredHotels.Select(h => MapToHotelDto(h, criteria.BeginDate, criteria.EndDate)).ToList();

        return new HotelsDTO { Hotels = hotelsDto };
    }

    public HotelDTO GetHotelForCriteria(CriteriaForHotel criteria)
    {
        Console.WriteLine("abcdf");
        var hotel = _hotelRepository.GetHotel(criteria.HotelId);

        if (hotel == null || !IsHotelAvailable(hotel, criteria.BeginDate, criteria.EndDate, criteria.NumberOfPeople))
        {
            return null;
        }

        return MapToHotelDto(hotel, criteria.BeginDate, criteria.EndDate);
    }
    
    // Pobieramy wszystkie rezerwacje dla danego hotelu, które zkładają się na przedział czasowy
    private bool IsHotelAvailable(Database.Entity.Hotel hotel, DateOnly beginDate, DateOnly endDate, int numberOfPeople)
    {
        var reservations = _hotelRepository.GetReservations()
            .Where(r => r.HotelId == hotel.Id &&
                        r.From < endDate && r.To > beginDate).ToList();

        var availableRooms = new List<HotelRoomType>();

        foreach (var room in hotel.Rooms)
        {
            var reservedRoomCount = reservations.SelectMany(r => r.Rooms)
                .Where(rr => rr.HotelRoomTypesId == room.RoomTypeId)
                .Sum(rr => rr.NumberOfRooms);

            // Liczba dostępnych pokoi jest różnicą między całkowitą liczbą pokoi a liczbą tych zarezerwowanych
            var availableCount = room.Count - reservedRoomCount;

            if (availableCount > 0)
            {
                availableRooms.Add(new HotelRoomType
                {
                    Count = availableCount,
                    PricePerNight = room.PricePerNight,
                    RoomTypeId = room.RoomTypeId
                });
            }
        }
        
        // Sumujemy liczbę miejsc dostępnych w wolnych pokojach i porównujemy ją z liczbą miesjc w kryterium
        return availableRooms.Sum(r => _hotelRepository.GetRoomType(r.RoomTypeId).NumberOfPeople * r.Count) >= numberOfPeople;
    }

    private HotelDTO MapToHotelDto(Database.Entity.Hotel hotel, DateOnly beginDate, DateOnly endDate)
    {
        return new HotelDTO
        {
            HotelId = hotel.Id,
            HotelName = hotel.Name,
            Country = hotel.Country,
            City = hotel.City,
            BeginDate = beginDate,
            EndDate = endDate,
            TypesOfMeals = hotel.Diets.Select(d => d.Name).ToList(),
            Discount = hotel.Discount,
            Rooms = hotel.Rooms.Select(r =>
            {
                var roomType = _hotelRepository.GetRoomType(r.RoomTypeId);
                return new RoomDTO
                {
                    Count = r.Count,
                    TypeOfRoom = roomType.Name,
                    NumberOfPeopleForTheRoom = roomType.NumberOfPeople,
                    PricePerRoom = r.PricePerNight
                };
            }).ToList()
        };
    }

    public Task<bool> ReserveHotel(HotelReservationDTO dto)
    {
        return _reservationRepository.ReserveAsync(dto.ReservationId, dto.HotelId, dto.BeginDate, dto.EndDate, dto.Rooms, dto.OfferId);
    }

    public Task CancelHotel(int offerId)
    {
        return _reservationRepository.CancelReservation(offerId);
    }

    public TopHotelsDTO GetTopHotels(int numberOfElements)
    {
        var topHotels = _hotelRepository.GetTopHotels(numberOfElements);
        return new TopHotelsDTO()
        {
            TopHotels = topHotels
        };
    }

    public TopRoomTypesDTO GetTopRoomTypes(int numberOfElements)
    {
        var topRoomTypes = _hotelRepository.GetTopRoomTypes(numberOfElements);
        return new TopRoomTypesDTO()
        {
            TopRoomTypes = topRoomTypes
        };
    }
}