import { FlightDTO } from './FlightDTO';
import { RoomDTO } from './RoomDTO';
import { TripDTO } from './TripDTO';
import { TripsDTO } from './TripsDTO';

export const rooms: RoomDTO[] = [
  {
    Count: 2,
    NumberOfPeopleForTheRoom: 1,
    PricePerRoom: 500,
    TypeOfRoom: '',
  },
];

export const flights: FlightDTO[] = [
  {
    Departure: 'Warszawa',
    DepartureTransportId: '',
    PricePerSeat: 100,
    ReturnTransportId: '',
  },
];

export const trips: TripDTO[] = [
  {
    BeginDate: new Date('2024-05-20'),
    EndDate: new Date('2024-05-27'),
    ChosenFlight: flights[0],
    City: 'Ateny',
    Country: 'Grecja',
    Discount: 0,
    HotelName: 'Hilton',
    PossibleFlights: flights,
    Rooms: rooms,
    TypesOfMeals: ['all inclusive'],
  },
  {
    BeginDate: new Date('2024-05-22'),
    EndDate: new Date('2024-05-29'),
    ChosenFlight: flights[0],
    City: 'Ateny',
    Country: 'Grecja',
    Discount: 0,
    HotelName: 'Sheraton',
    PossibleFlights: flights,
    Rooms: rooms,
    TypesOfMeals: ['all inclusive'],
  },
];

export const tripslists: TripsDTO[] = [{ Trips: trips }];
