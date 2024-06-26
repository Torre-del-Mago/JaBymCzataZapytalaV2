import { FlightDTO } from './model/FlightDTO';
import { RoomDTO } from './model/RoomDTO';
import { TripDTO } from './model/TripDTO';
import { TripsDTO } from './model/TripsDTO';

export const rooms: RoomDTO[] = [
  {
    id:1,
    count: 2,
    numberOfPeopleForTheRoom: 2,
    pricePerRoom: 200,
    typeOfRoom: 'Cowabunga',
  },
  {
    id:2,
    count: 2,
    numberOfPeopleForTheRoom: 1,
    pricePerRoom: 500,
    typeOfRoom: 'Funky Monkey',
  },
  {
    id:3,
    count: 2,
    numberOfPeopleForTheRoom: 1,
    pricePerRoom: 300,
    typeOfRoom: 'Chilled dog',
  },
];
export const dummyRoom: RoomDTO = 
  {
    id: 0,
    count: 0,
    numberOfPeopleForTheRoom: 0,
    pricePerRoom: 500,
    typeOfRoom: '',
  };

export const flights: FlightDTO[] = [
  {
    departure: 'Warszawa',
    departureTransportId: '',
    pricePerSeat: 100,
    returnTransportId: '',
  },
  {
    departure: 'Kraków',
    departureTransportId: '',
    pricePerSeat: 80,
    returnTransportId: '',
  }
];

export const trips: TripDTO[] = [
  {
    hotelId: 1,
    beginDate: new Date('2024-05-20'),
    endDate: new Date('2024-05-27'),
    chosenFlight: flights[0],
    city: 'Ateny',
    country: 'Grecja',
    discount: 0,
    hotelName: 'Hilton',
    possibleFlights: flights,
    rooms: rooms,
    typesOfMeals: ['all inclusive'],
  },
  {
    hotelId: 2,
    beginDate: new Date('2024-05-22'),
    endDate: new Date('2024-05-29'),
    chosenFlight: flights[0],
    city: 'Ateny',
    country: 'Grecja',
    discount: 0,
    hotelName: 'Sheraton',
    possibleFlights: flights,
    rooms: rooms,
    typesOfMeals: ['all inclusive'],
  },
];

export const tripslists: TripsDTO[] = [{ trips: trips }];
