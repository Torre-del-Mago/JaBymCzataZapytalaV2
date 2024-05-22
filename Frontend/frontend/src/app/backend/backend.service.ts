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

  private dates: string[] = [];

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
      if (roomCombinations.length > 0) {
        let defaultRooms: RoomDTO[] = Array(
          numberOfChildren + numberOfAdults
        ).fill(Mocks.dummyRoom);
        for (const [index, value] of roomCombinations.entries()) {
          if (value > 0) {
            let foundRoom = hotel.Rooms.find(
              (r) => r.NumberOfPeopleForTheRoom == index + 1 && r.Count >= value
            );
            if (foundRoom !== undefined) {
              defaultRooms[index + 1] = foundRoom;
            }
          }
        }
        let price =
          hotel.ChosenFlight.PricePerSeat * (numberOfAdults + numberOfChildren);
        let timediff = endDate.getTime() - startDate.getTime();
        let days = Math.round(timediff / (1000 * 3600 * 24));
        for (const [index, defroom] of defaultRooms.entries()) {
          if (roomCombinations[index] > 0) {
            price += defroom.PricePerRoom * days * roomCombinations[index];
          }
        }
        trips.push({
          ...hotel,
          RoomCombination: roomCombinations,
          ChosenRooms: defaultRooms,
          Price: price,
        });
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

  public setDates(begin: string, end: string): void {
    this.dates = [begin, end];
  }

  public setNumbers(numOfChild: number, numOfAdult: number) {
    this.numOfAdult = numOfAdult;
    this.numOfChildren = numOfChild;
  }

  public getNumbers(): number[] {
    return [this.numOfAdult, this.numOfChildren];
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
        (r) => r.NumberOfPeopleForTheRoom === numberOfPeople
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
        numbers[numberOfPeople - 1 - indexOfPeople] += 1;
      } else {
        numbers[indexOfPeople] = Math.floor(numberOfPeople / numOfPeople);
        numbers[(numberOfPeople % numOfPeople) - 1] = 1;
      }
      //check for rooms
      let areAllFree = true;
      for (const [index, value] of numbers.entries()) {
        if (value > 0) {
          const room = rooms.find(
            (r) => r.NumberOfPeopleForTheRoom == index + 1 && r.Count >= value
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
