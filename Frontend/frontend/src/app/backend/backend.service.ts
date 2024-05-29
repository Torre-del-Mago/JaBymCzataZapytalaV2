import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Mocks from '../dto/Mocks';
import { TripDTO } from '../dto/TripDTO';
import { RoomDTO } from '../dto/RoomDTO';
import {Observable} from 'rxjs'

interface RoomSize {
  Size: number;
  Count: number;
}

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private client: HttpClient) {}

  //Nie wiem czy tu jest http czy https więc zmieńcie jak coś
  //Nie wiem jaki port czy też jak nazwiecie bramę w dockerze
  private gateUrl = 'https://localhost:55278/api';
  private loginCheckUrl = '/login/check';
  private tripControllerUrl = '/trip';
  private tripListUrl = '/trip-list-info';
  private tripUrl = '/trip-info';
  private tripsTestUrl = '/test-get'

  private currentTrip?: TripDTO;
  private numOfChildren: number = 0;
  private numOfAdults: number = 0;
  private tripToReserve?: TripDTO;

  private dates: string[] = [];

  public checkLogin(userName: string): boolean {
    return ['jasio', 'kasia', 'zbysio'].includes(userName.trim())
      ? true
      : false;
  }

  public getInfoForTrips(
    destination: string,
    startCity: string,
    startDate: string,
    endDate: string,
    numberOfAdults: number,
    numberOfChildren: number
  ): Observable<TripDTO[]> {

    return this.client.get<TripDTO[]>(this.gateUrl + this.tripControllerUrl + this.tripsTestUrl + "?destination=" + destination 
    + "&departure=" + startCity + "&numberOfPeople=" + (numberOfAdults+numberOfChildren) + "&startDate=" + startDate + 
    "&endDate=" + endDate);

    /*
    /* return this.client.get<TripsDTO>(this.gateUrl + this.tripListUrl + "?body=") */
  }

  public changeTrips(trips: TripDTO[], startDate: string, endDate: string, numberOfAdults: number, numberOfChildren: number): TripDTO[] {
    let result: TripDTO[] = []
    for (const hotel of trips) {
      const roomCombinations = this.calculateRoomCombinations(
        hotel.rooms,
        numberOfAdults + numberOfChildren,
        true
      );
      if (roomCombinations.length > 0) {
        let defaultRooms: RoomDTO[] = Array(
          numberOfChildren + numberOfAdults
        ).fill(Mocks.dummyRoom);
        for (const [index, value] of roomCombinations.entries()) {
          if (value > 0) {
            let foundRoom = hotel.rooms.find(
              (r) => r.numberOfPeopleForTheRoom == index + 1 && r.count >= value
            );
            if (foundRoom !== undefined) {
              defaultRooms[index + 1] = foundRoom;
            }
          }
        }
        let price =
          hotel.chosenFlight.pricePerSeat * (numberOfAdults + numberOfChildren);
        console.log(hotel.beginDate)
        console.log(hotel.endDate)
        let timediff = new Date(hotel.endDate).getTime() - new Date(hotel.beginDate).getTime();
        let days = Math.round(timediff / (1000 * 3600 * 24));
        for (const [index, defroom] of defaultRooms.entries()) {
          if (roomCombinations[index] > 0) {
            price += defroom.pricePerRoom * days * roomCombinations[index];
          }
        }
        result.push({
          ...hotel,
          roomCombination: roomCombinations,
          chosenRooms: defaultRooms,
          price: price,
        });
      }
    }
    return result;
  }

  public getInfoForTrip(
    destination: string,
    startCity: string,
    startDate: Date,
    endDate: Date,
    numberOfAdults: number,
    numberOfChildren: number,
    selectedHotel: string | undefined = undefined
  ): TripDTO | null {
    var trip: TripDTO | null = null;
    const hotel = Mocks.trips.find(
      (t) =>
        t.country === destination &&
        t.chosenFlight.departure === startCity &&
        t.beginDate >= startDate &&
        t.endDate <= endDate &&
        t.hotelName === (selectedHotel ?? t.hotelName)
    );

    if(hotel === undefined) {
      return null;
    }

    const roomCombinations = this.calculateRoomCombinations(
      hotel.rooms,
      numberOfAdults + numberOfChildren,
      true
    );
    if (roomCombinations.length > 0) {
      let defaultRooms: RoomDTO[] = Array(
        numberOfChildren + numberOfAdults
      ).fill(Mocks.dummyRoom);
      for (const [index, value] of roomCombinations.entries()) {
        if (value > 0) {
          let foundRoom = hotel.rooms.find(
            (r: RoomDTO) => r.numberOfPeopleForTheRoom == index + 1 && r.count >= value
          );
          if (foundRoom !== undefined) {
            defaultRooms[index + 1] = foundRoom;
          }
        }
      }
      let price =
        hotel.chosenFlight.pricePerSeat * (numberOfAdults + numberOfChildren);
      let timediff = new Date(hotel.endDate).getTime() - new Date(hotel.beginDate).getTime();
      let days = Math.round(timediff / (1000 * 3600 * 24));
      console.log(days);
      for (const [index, defroom] of defaultRooms.entries()) {
        if (roomCombinations[index] > 0) {
          price += defroom.pricePerRoom * days * roomCombinations[index];
        }
      }
      trip = {
        ...hotel,
        roomCombination: roomCombinations,
        chosenRooms: defaultRooms,
        price: price,
      } as TripDTO;
    }
    return trip;
  }

  public reserveOffer(
    trip: TripDTO,
    numOfAdults: number,
    numOfChildren: number
  ) {
    this.tripToReserve = trip;
    this.numOfAdults = numOfAdults;
    this.numOfChildren = numOfChildren;
  }

  public getReservedOffer(): TripDTO {
    return this.tripToReserve!;
  }

  public payOffer() {}

  public setCurrentTrip(trip: TripDTO): void {
    this.currentTrip = trip;
  }

  public setDates(begin: string, end: string): void {
    this.dates = [begin, end];
  }

  public setNumbers(numOfChild: number, numOfAdult: number) {
    this.numOfAdults = numOfAdult;
    this.numOfChildren = numOfChild;
  }

  public getNumbers(): number[] {
    return [this.numOfAdults, this.numOfChildren];
  }

  public getDates(): string[] {
    return this.dates;
  }

  public getCurrentTrip(): TripDTO | undefined {
    return this.currentTrip;
  }

  private calculateRoomCombinations(
    rooms: RoomDTO[],
    numberOfPeople: number,
    exactNumberMatch: boolean
  ): number[] {
    let combinations: number[] = [];
    console.log(numberOfPeople);
    // find rooms that exactly match numberOfPeople - only if exactNumberMatch === true
    if (exactNumberMatch) {
      const exactMatchRooms = rooms.find(
        (r) => r.numberOfPeopleForTheRoom === numberOfPeople
      );
      if (exactMatchRooms !== undefined) {
        combinations = Array(numberOfPeople).fill(0);
        combinations[numberOfPeople - 1] = 1;
      }
    }
    if (combinations.length === 0) {
      let numbers = this.getRooms(rooms, numberOfPeople);
      if (numbers.length > 0) {
        combinations = numbers;
      }
    }
    return combinations;
  }

  private getRooms(rooms: RoomDTO[], numberOfPeople: number): number[] {
    let numbers = Array(numberOfPeople).fill(0);
    for (let numOfPeople = numberOfPeople; numOfPeople > 0; numOfPeople--) {
      numbers = Array(numberOfPeople).fill(0);
      console.log(numOfPeople);
      let indexOfPeople = numOfPeople - 1;
      if (numOfPeople == numberOfPeople) {
        numbers[indexOfPeople] = 1;
      } else if (numOfPeople == 1) {
        numbers[indexOfPeople] = numberOfPeople;
        console.log(numbers);
        console.log(numbers);
      } else if (numOfPeople >= Math.ceil(numberOfPeople / 2)) {
        numbers[indexOfPeople] = 1;
        numbers[(numberOfPeople - 1) - numOfPeople] += 1;
      } else {
        numbers[indexOfPeople] = Math.floor(numberOfPeople / numOfPeople);
        numbers[(numberOfPeople % numOfPeople) - 1] = 1;
      }
      //check for rooms
      let areAllFree = true;
      for (const [index, value] of numbers.entries()) {
        if (value > 0) {
          const room = rooms.find(
            (r) => r.numberOfPeopleForTheRoom == index + 1 && r.count >= value
          );
          if (room === undefined) {
            areAllFree = false;
          }
        }
      }
      if (areAllFree) {
        return numbers;
      }
    }
    return [];
  }
}
