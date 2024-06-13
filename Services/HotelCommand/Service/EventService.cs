using HotelCommand.Database.Tables;
using HotelCommand.Repository.DietRepository;
using HotelCommand.Repository.HotelDietRepository;
using HotelCommand.Repository.HotelRepository;
using HotelCommand.Repository.HotelRoomTypeRepository;
using HotelCommand.Repository.ReservationEventRepository;
using HotelCommand.Repository.ReservationRepository;
using HotelCommand.Repository.ReservedRoomRepository;
using Models.Hotel.DTO;

namespace HotelCommand.Service;

public class EventService : IEventService
{
    private IReservedRoomRepository _reservedRoomRepository;
    private IReservationRepository _reservationRepository;
    private IReservationEventRepository _eventRepository;
    private IHotelRepository _hotelRepository;
    private IHotelRoomTypeRepository _hotelRoomTypeRepository;
    private IDietRepository _dietRepository;
    private IHotelDietRepository _hotelDietRepository;

    public EventService(IReservedRoomRepository reservedRoomRepository, IReservationRepository reservationRepository,
        IReservationEventRepository eventRepository, IHotelRepository hotelRepository,
        IHotelRoomTypeRepository hotelRoomTypeRepository, IDietRepository dietRepository, IHotelDietRepository hotelDietRepository)
    {
        _reservedRoomRepository = reservedRoomRepository;
        _reservationRepository = reservationRepository;
        _eventRepository = eventRepository;
        _hotelRepository = hotelRepository;
        _hotelRoomTypeRepository = hotelRoomTypeRepository;
        _dietRepository = dietRepository;
        _hotelDietRepository = hotelDietRepository;
    }

    public async Task<bool> ReserveHotel(HotelReservationDTO dto)
    {
        var hotelById = await _hotelRepository.GetHotelByIdAsync(dto.HotelId);
        if (hotelById == null)
        {
            return false;
        }

        var overlappingReservations = await _reservationRepository
            .GetReservationByHotelIdDatesAndNotDeleted(hotelById.Id, dto.BeginDate, dto.EndDate);

        var reservedRoomsCount = new Dictionary<int, int>();
        
        foreach (var reservation in overlappingReservations)
        {
            foreach (var reservedRoom in reservation.Rooms)
            {
                var roomType = await _hotelRoomTypeRepository.GetHotelRoomTypeByIdAsync(reservedRoom.HotelRoomTypeId);
                reservedRoomsCount.TryAdd(roomType.RoomTypeId, 0);
                reservedRoomsCount[roomType.RoomTypeId] += reservedRoom.NumberOfRooms;
            }
        }
        foreach (var room in dto.Rooms)
        {
            var roomType = await _hotelRoomTypeRepository.GetHotelRoomTypeByNameAsync(room.TypeOfRoom);
            if (roomType == null)
            {
                return false; // Typ pokoju nie istnieje
            }
            if (reservedRoomsCount.TryGetValue(roomType.RoomTypeId, out int reservedCount))
            {
                if (reservedCount + room.Count > roomType.NumberOfRooms)
                {
                    return false; // Brak dostępnych pokoi
                }
            }
        }
        
        var newReservation = new Reservation
        {
            From = dto.BeginDate,
            To = dto.EndDate,
            HotelId = dto.HotelId,
            Rooms = new List<ReservedRoom>(),
            OfferId = dto.OfferId
        };

        // Tworzenie obiektów ReservedRoom dla nowej rezerwacji
        foreach (var room in dto.Rooms)
        {
            var roomType = await _hotelRoomTypeRepository.GetHotelRoomTypeByNameAsync(room.TypeOfRoom);
            if (roomType == null)
            {
                return false; // Typ pokoju nie istnieje
            }

            var reservedRoom = new ReservedRoom
            {
                HotelRoomTypeId = roomType.Id,
                NumberOfRooms = room.Count,
                Reservation = newReservation
            };

            _reservedRoomRepository.InsertReservedRoom(reservedRoom);
            newReservation.Rooms.Add(reservedRoom);
        }
        //_reservationRepository.InsertReservation(newReservation);
        
        await _eventRepository.InsertReservationEvent(newReservation.Id);
        return true;
    }

    public async Task CancelHotel(int offerId)
    {
        await _eventRepository.InsertCancellationEvent(offerId);
    }

    public async Task AddDiet(int hotelId, int dietId)
    {
        var hotelToChange = await _hotelRepository.GetHotelByIdAsync(hotelId);
        if (!hotelToChange.HotelDiets.Select(d => d.DietId).Contains(dietId))
        {
            var diet = await _dietRepository.GetDietByIdAsync(dietId);
            var hotelDietToAdd = new HotelDiet
                { Diet = diet, DietId = diet.Id, Hotel = hotelToChange, HotelId = hotelToChange.Id, };

            diet.HotelDiets.Add(hotelDietToAdd);
            hotelToChange.HotelDiets.Add(hotelDietToAdd);

            _hotelDietRepository.AddHotelDiet(hotelDietToAdd);
            _dietRepository.UpdateDiet(diet);
            _hotelRepository.UpdateHotel(hotelToChange);
        }
    }

    public double ChangeHotelDiscount(int hotelId, double discountChange)
    {
        var hotel = _hotelRepository.GetHotel(hotelId);
        double newDiscount = Convert.ToDouble(discountChange);
        double currentDiscount = Convert.ToDouble(hotel.Discount);
        if ((currentDiscount + discountChange) < 0)
        {
            newDiscount = currentDiscount;
        }
        else if((currentDiscount + discountChange) >= 1.0)
        {
            newDiscount = (1.0) - currentDiscount;
        }
        _hotelRepository.UpdateDiscount(hotelId, Convert.ToSingle(newDiscount));
        return newDiscount;
    }
}