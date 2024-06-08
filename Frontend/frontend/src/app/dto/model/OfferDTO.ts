import { FlightDTO } from './FlightDTO';
import { RoomDTO } from './RoomDTO';
export type OfferDTO = {
    hotelId: number;
    country: string;
    city: string;
    beginDate: Date;
    endDate: Date;
    flight: FlightDTO;
    typeOfMeal: string;
    rooms: RoomDTO[];
    numberOfAdults: number;
    numberOfNewborns: number;
    numberOfToddlers: number;
    numberOfTeenagers: number;  
}