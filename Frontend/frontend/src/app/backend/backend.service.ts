import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Mocks from '../dto/Mocks';
import { TripDTO } from '../dto/model/TripDTO';
import { RoomDTO } from '../dto/model/RoomDTO';
import { GenerateTripResponse } from '../dto/response/GenerateTripResponse';
import { GenerateTripsResponse } from '../dto/response/GenerateTripsResponse';
import { PayResponse } from '../dto/response/PayResponse';
import { ReserveOfferResponse } from '../dto/response/ReserveOfferResponse';
import { ReserveOfferRequest } from '../dto/request/ReserveOfferRequest';
import { OfferDTO } from '../dto/model/OfferDTO';
import { Observable, pipe, map } from 'rxjs';

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
  private gateUrl = 'http://localhost:55278/api';
  private loginCheckUrl = '/logging/check';
  private testPaymentUrl = '/payment/check';
  private testReserveUrl = '/offer/reserve';
  private tripControllerUrl = '/trip';
  private tripListUrl = '/trip-list-info';
  private tripUrl = '/trip-info';
  private tripsTestUrl = '/trip-list-info';
  private tripSingleTestUrl = '/trip-info';
  private loggedInUser: string = '';

  private currentTrip?: TripDTO;
  private numOfChildren: number = 0;
  private numOfAdults: number = 0;
  private tripToReserve?: TripDTO;
  private offerInfo?: ReserveOfferResponse;

  private dates: string[] = [];

  public tryPaying(offerId: number, amount: number): Observable<PayResponse> {
    return this.client.post<PayResponse>(
      this.gateUrl +
        this.testPaymentUrl +
        '?offerId=' +
        offerId +
        '&amount=' +
        amount,
      {}
    );
  }

  public checkLogin(userName: string): Observable<Object> {
    console.log(userName)
    return this.client.get(
      this.gateUrl + this.loginCheckUrl + '?login=' + userName
    );
  }

  public getInfoForTrips(
    destination: string,
    startCity: string,
    startDate: string,
    endDate: string,
    numberOfAdults: number,
    numberOfChildren: number
  ): Observable<TripDTO[]> {
    return this.client.get<GenerateTripsResponse>(
      this.gateUrl +
        this.tripControllerUrl +
        this.tripsTestUrl +
        '?destination=' +
        destination +
        '&departure=' +
        startCity +
        '&numberOfPeople=' +
        (numberOfAdults + numberOfChildren) +
        '&beginDate=' +
        startDate +
        '&endDate=' +
        endDate
    ).pipe(map((r: GenerateTripsResponse) => {
      return r.trips.trips;
    }));
    

    /*
    /* return this.client.get<TripsDTO>(this.gateUrl + this.tripListUrl + "?body=") */
  }

  public getInfoForTrip(
    destination: string,
    startCity: string,
    startDate: string,
    endDate: string,
    numberOfAdults: number,
    numberOfChildren: number
  ): Observable<TripDTO[]> {
    return this.client.get<TripDTO[]>(
      this.gateUrl +
        this.tripControllerUrl +
        this.tripsTestUrl +
        '?destination=' +
        destination +
        '&departure=' +
        startCity +
        '&numberOfPeople=' +
        (numberOfAdults + numberOfChildren) +
        '&startDate=' +
        startDate +
        '&endDate=' +
        endDate
    );
    

    /*
    /* return this.client.get<TripsDTO>(this.gateUrl + this.tripListUrl + "?body=") */
  }

  public changeTrips(
    trips: TripDTO[],
    startDate: string,
    endDate: string,
    numberOfAdults: number,
    numberOfChildren: number,
    isTripSingle: boolean = false
  ): TripDTO[] {
    console.log(trips)
    let result: TripDTO[] = [];
    for (const hotel of trips) {
      console.log(hotel)
      console.log(hotel.rooms)
      const roomCombinations = this.calculateRoomCombinations(
        !isTripSingle ? hotel.rooms: hotel.rooms,
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
        console.log(hotel.beginDate);
        console.log(hotel.endDate);
        let timediff =
          new Date(hotel.endDate).getTime() -
          new Date(hotel.beginDate).getTime();
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

  public getInfoForTripNew(
    destination: string,
    country: string,
    startCity: string,
    startDate: Date,
    endDate: Date,
    numberOfAdults: number,
    numberOfChildren: number,
    selectedHotelId: number
  ): Observable<GenerateTripResponse> {

    return this.client.get<GenerateTripResponse>(
      this.gateUrl +
        this.tripControllerUrl +
        this.tripSingleTestUrl +
        '?destination=' +
        destination +
        '?country=' +
        country +
        '&departure=' +
        startCity +
        '&numberOfPeople=' +
        (numberOfAdults + numberOfChildren) +
        '&beginDate=' +
        startDate +
        '&endDate=' +
        endDate +
        "&hotelId=" +
        selectedHotelId
    );
  }

  public reserveOffer(
    trip: TripDTO,
    numOfAdults: number,
    numOfChildren: number
  ): Observable<ReserveOfferResponse> {
    this.tripToReserve = trip;
    this.numOfAdults = numOfAdults;
    this.numOfChildren = numOfChildren;
    let transformedRooms: RoomDTO[] = [];
    for (const [index, value] of trip.roomCombination!.entries()) {
      if (value > 0) {
        console.log(trip.roomCombination);
        console.log(trip.chosenRooms);
        const count = transformedRooms.push(trip.chosenRooms![index + 1]);
        transformedRooms[count - 1].count = value;
      }
    }
    console.log({
      hotelId: trip.hotelId,
      country: trip.country,
      city: trip.city,
      beginDate: trip.beginDate,
      endDate: trip.endDate,
      flight: trip.chosenFlight,
      typeOfMeal: trip.chosenMeal,
      rooms: transformedRooms,
      numberOfAdults: numOfAdults,
      numberOfNewborns: 0,
      numberOfToddlers: 0,
      numberOfTeenagers: numOfChildren,
    });
    return this.client.post<ReserveOfferResponse>(
      this.gateUrl + this.testReserveUrl,
      {
        registration: 0,
        offer: {
          hotelId: trip.hotelId,
          country: trip.country,
          city: trip.city,
          beginDate: trip.beginDate,
          endDate: trip.endDate,
          flight: trip.chosenFlight,
          typeOfMeal: trip.chosenMeal,
          rooms: transformedRooms,
          numberOfAdults: numOfAdults,
          numberOfNewborns: 0,
          numberOfToddlers: 0,
          numberOfTeenagers: numOfChildren,
        } as OfferDTO,
      } as ReserveOfferRequest
    );
  }

  public getReservedOffer(): TripDTO {
    return this.tripToReserve!;
  }

  public getOfferInfo(): ReserveOfferResponse {
    return this.offerInfo!;
  }

  public getUser(): string {
    return this.loggedInUser;
  }

  public setUser(username: string): void {
    this.loggedInUser = username;
  }

  public setOfferInfo(offerInfo: ReserveOfferResponse): void {
    this.offerInfo = offerInfo;
  }

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
        numbers[numberOfPeople - 1 - numOfPeople] += 1;
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
