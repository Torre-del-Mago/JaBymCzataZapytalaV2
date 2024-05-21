import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Mocks from '../dto/Mocks';
import { TripDTO } from '../dto/TripDTO';
import { RoomDTO } from '../dto/RoomDTO';

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
  private gateUrl = 'http://gate:8080/api';
  private loginCheckUrl = '/login/check';
  private tripControllerUrl = '/trip';
  private tripListUrl = '/trip-list-info';
  private tripUrl = '/trip-info';

  private currentTrip?: TripDTO;
  private numOfChildren: number = 0;
  private numOfAdult: number = 0;

  public checkLogin(userName: string): boolean {
    return ['jasio', 'kasia', 'zbysio'].includes(userName.trim())
      ? true
      : false;
  }

  public getInfoForTrips(
    destination: string,
    startCity: string,
    startDate: Date,
    endDate: Date,
    numberOfAdults: number,
    numberOfChildren: number
  ): TripDTO[] {
    var trips: TripDTO[] = [];
    const hotels = Mocks.trips.filter(
      (t) =>
        t.Country === destination &&
        t.ChosenFlight.Departure === startCity &&
        t.BeginDate >= startDate &&
        t.EndDate <= endDate
    );

    for (const hotel of hotels) {
      console.log(hotel);
      const roomCombinations = this.calculateRoomCombinations(
        hotel.Rooms,
        numberOfAdults + numberOfChildren,
        true
      );
      console.log(roomCombinations);
      if(roomCombinations.length > 0) {
        trips.push({ ...hotel, RoomCombination: roomCombinations });
      }
    }

    return trips;
    /* return this.client.get<TripsDTO>(this.gateUrl + this.tripListUrl + "?body=") */
  }

  public getInfoForTrip() {}

  public reserveOffer() {}

  public payOffer() {}

  public setCurrentTrip(trip: TripDTO): void {
    this.currentTrip = trip;
  }

  public setNumbers(numOfChild: number, numOfAdult: number) {
    this.numOfAdult = numOfAdult;
    this.numOfChildren = numOfChild;
  }

  public getNumbers(): number[] {
    return [this.numOfAdult, this.numOfChildren];
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
        (r) => r.NumberOfPeopleForTheRoom === numberOfPeople
      );
      if (exactMatchRooms !== undefined) {
        combinations = Array(numberOfPeople).fill(0);
        console.log(exactMatchRooms)
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
      let indexOfPeople = numOfPeople - 1;
      if (numOfPeople == numberOfPeople) {
        numbers[indexOfPeople] = 1;
      } else if (numOfPeople >= Math.ceil(numberOfPeople / 2)) {
        numbers[indexOfPeople] = 1;
        numbers[numberOfPeople - 1 - indexOfPeople] = 1;
      } else if (numOfPeople == 1) {
        numbers[indexOfPeople] == numberOfPeople;
      } else {
        numbers[indexOfPeople] = Math.floor(numberOfPeople / numOfPeople);
        numbers[(numberOfPeople % numOfPeople) - 1] = 1;
      }
      //check for rooms
      let areAllFree = true;
      for (const [index, value] of numbers.entries()) {
        if (value > 0) {
          console.log("Got value" + value + " at index " + index);
          const room = rooms.find(
            (r) => r.NumberOfPeopleForTheRoom == (index + 1) && r.Count >= value
          );
          console.log(room);
          if (room === undefined) {
            areAllFree = false;
          }
          else {
            console.log(room)
          }
        }
      }
      if (areAllFree) {
        console.log("Found numbers")
        return numbers;
      }
    }
    return [];
  }
}
