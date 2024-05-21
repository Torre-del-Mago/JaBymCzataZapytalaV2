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
    const trips: TripDTO[] = [];
    const hotels = Mocks.trips.filter(
      (t) =>
        t.Country === destination &&
        t.ChosenFlight.Departure === startCity &&
        t.BeginDate >= startDate &&
        t.EndDate <= endDate
    );
    for (const hotel of hotels) {
      const roomCombinations = this.calculateRoomCombinations(
        hotel.Rooms,
        numberOfAdults + numberOfChildren,
        true
      );
      roomCombinations.forEach((c) => trips.push({ ...hotel, ChosenRooms: c }));
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
  ): RoomDTO[][] {
    let combinations: RoomDTO[][] = [];
    // find rooms that exactly match numberOfPeople - only if exactNumberMatch === true
    if (exactNumberMatch) {
      const exactMatchRooms = rooms.filter(
        (r) => r.NumberOfPeopleForTheRoom === numberOfPeople
      );
      exactMatchRooms.forEach((r) => combinations.push([r]));
    }
    if (combinations.length === 0) {
      // get all room sizes
      const roomSizes = Array.from(
        new Set(rooms.map((r) => r.NumberOfPeopleForTheRoom))
      );
      // loop by rising number of rooms, start with 2, step 1, finish with number of people
      for (
        let numberOfRooms = 2;
        numberOfRooms <= numberOfPeople;
        numberOfRooms++
      ) {
        // calculate all possible combinations of room sizes for given number of rooms
        const roomSizecombinations = new Set(
          this.calculateRoomSizesCombinations(
            this.getRoomSizes(rooms),
            numberOfPeople,
            numberOfRooms
          )
        );
        // if for given number of rooms there's a combination, get rooms and break loop
        if (roomSizecombinations.size > 0) {
          // get rooms combinations from room sizes combinations
          // TODO magic happens here
          // break
          break;
        }
      }
      // if still nothing is found - run a loop with recursive calls from numberOfPeople+1 to numberOfPeople * 2 with exactNumberMatch === false
      for (
        let numberOfPlaces = numberOfPeople + 1;
        numberOfPlaces <= numberOfPeople * 2;
        numberOfPlaces++
      ) {
        combinations = this.calculateRoomCombinations(
          rooms,
          numberOfPlaces,
          false
        );
        if (combinations.length > 0) break;
      }
    }
    return combinations;
  }

  private calculateRoomSizesCombinations(
    roomSizes: RoomSize[],
    numberOfPeople: number,
    numberOfRooms: number
  ): RoomSize[][] {
    if (numberOfRooms === 1) {
      const room = roomSizes.find((r) => r.Size === numberOfPeople);
      if (room !== undefined) return [[room]];
      else return [];
    } else {
      const result: RoomSize[][] = [];
      for (const roomSize of roomSizes.filter(
        (rs) => rs.Count > 0 && rs.Size <= numberOfPeople
      )) {
        const copyOfRoomSizes = this.cloneObjects(roomSizes);
        const roomSizeToUpdate = copyOfRoomSizes.find(
          (rs) => rs.Size === roomSize.Size
        );
        if (roomSizeToUpdate !== undefined) {
          roomSizeToUpdate.Count--;
          const subResult = this.calculateRoomSizesCombinations(
            copyOfRoomSizes,
            numberOfPeople - roomSizeToUpdate.Size,
            numberOfRooms - 1
          );
          if (subResult.length > 0) {
            for (const subResultItem of subResult) {
              const roomInSubResult = subResultItem.find(
                (rs) => rs.Size === roomSizeToUpdate.Size
              );
              if (roomInSubResult !== undefined) {
                roomInSubResult.Count++;
              } else {
                subResultItem.push({ Size: roomSizeToUpdate.Size, Count: 1 });
              }
              result.push(subResultItem);
            }
          }
        }
      }
      return result;
    }
  }

  private getRoomSizes(rooms: RoomDTO[]): RoomSize[] {
    const roomSizes: RoomSize[] = [];
    const map: Map<number, number> = new Map();
    for (const room of rooms) {
      let val = map.get(room.NumberOfPeopleForTheRoom) ?? 0;
      val += room.Count;
      map.set(room.NumberOfPeopleForTheRoom, val);
    }
    return Array.from(map.entries()).map((v) => ({ Size: v[0], Count: v[1] }));
  }

  // there are libraries for it, but why not code it...
  private cloneObjects<T>(objects: T[]): T[] {
    return JSON.parse(JSON.stringify(objects));
  }
}
