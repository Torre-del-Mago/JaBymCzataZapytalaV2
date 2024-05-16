import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor() {}

  public checkLogin(userName: string): boolean {
    return ['jasio', 'kasia', 'zbysio'].includes(userName.trim())
      ? true
      : false;
  }

  public getInfoForTrips() {}

  public getInfoForTrip() {}

  public reserveOffer() {}

  public payOffer() {}
}
