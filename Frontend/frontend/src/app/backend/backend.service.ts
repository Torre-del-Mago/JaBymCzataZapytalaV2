import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Mocks from '../dto/Mocks';
import { TripDTO } from '../dto/TripDTO';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(/*private client: HttpClient*/) {}

  //Nie wiem czy tu jest http czy https więc zmieńcie jak coś
  //Nie wiem jaki port czy też jak nazwiecie bramę w dockerze
  private gateUrl = 'http://gate:8080/api';
  private loginCheckUrl = '/login/check';
  private tripControllerUrl = '/trip';
  private tripListUrl = '/trip-list-info';
  private tripUrl = '/trip-info';

  private currentTrip?: TripDTO
  private numberOfChildren: number = 0
  private numberOfAdult: number = 0

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
    return Mocks.trips.filter(
      (t: TripDTO) =>
        t.Country === destination &&
        t.ChosenFlight.Departure === startCity &&
        t.BeginDate >= startDate &&
        t.EndDate <= endDate
    );
    /* return this.client.get<TripsDTO>(this.gateUrl + this.tripListUrl + "?body=") */
  }

  public getInfoForTrip() {}

  public reserveOffer() {}

  public payOffer() {}

  public setCurrentTrip(trip: TripDTO): void {
    this.currentTrip = trip;
  }

  public setNumbers(numberOfChild: number, numberOfAdult: number) {
    this.numberOfAdult = numberOfAdult
    this.numberOfChildren = numberOfChild
  }

  public getNumbers(): number[] {
    return [this.numberOfAdult, this.numberOfChildren]
  }

  public getCurrentTrip(): TripDTO | undefined {
    return this.currentTrip;
  }
}
