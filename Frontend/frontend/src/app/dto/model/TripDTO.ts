import { FlightDTO } from './FlightDTO';
import { RoomDTO } from './RoomDTO';
export type TripDTO = {
  hotelId: number;
  hotelName: string;
  country: string;
  city: string;
  beginDate: Date;
  endDate: Date;
  typesOfMeals: string[];
  discount: number;
  roomCombination?: number[];
  rooms: RoomDTO[];
  chosenFlight: FlightDTO;
  possibleFlights: FlightDTO[];
  chosenRooms?: RoomDTO[];
  chosenMeal?: string
  price?: number
};
