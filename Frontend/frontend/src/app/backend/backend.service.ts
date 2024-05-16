import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor() {}
  //Nie wiem czy tu jest http czy https więc zmieńcie jak coś
  //Nie wiem jaki port czy też jak nazwiecie bramę w dockerze
  private gateUrl = "http://gate:8080/api"
  private loginCheckUrl = "/login/check"
  private tripControllerUrl = "/trip"
  private tripListUrl = "/trip-list-info"
  private tripUrl = "/trip-info"

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
