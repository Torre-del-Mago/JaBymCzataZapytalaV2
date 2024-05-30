using HotelCommand.Database.Tables;
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
        
    public EventService(IReservedRoomRepository reservedRoomRepository, IReservationRepository reservationRepository, IReservationEventRepository eventRepository, IHotelRepository hotelRepository, IHotelRoomTypeRepository hotelRoomTypeRepository)
    {
        _reservedRoomRepository = reservedRoomRepository;
        _reservationRepository = reservationRepository;
        _eventRepository = eventRepository;
        _hotelRepository = hotelRepository;
        _hotelRoomTypeRepository = hotelRoomTypeRepository;
    }
    
    public async Task<bool> ReserveHotel(HotelReservationDTO dto)
    {
        var hotelById = _hotelRepository.GetHotelByIdAsync(dto.HotelId);
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
            Rooms = new List<ReservedRoom>()
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
        _reservationRepository.InsertReservation(newReservation);
        
        await _eventRepository.InsertReservationEvent(newReservation.Id);
        return true;
    }

    public async Task CancelHotel(int offerId)
    {
        await _eventRepository.InsertCancellationEvent(offerId);
    }
}