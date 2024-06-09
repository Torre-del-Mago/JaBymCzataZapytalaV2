import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable, of, pipe, map} from 'rxjs'
import {AdminTransportInfoDTO} from '../dto/real-time/AdminTransportInfoDTO'
import {AdminRoomTypeInfoDTO} from '../dto/real-time/AdminRoomTypeInfoDTO'
import {AdminHotelInfoDTO} from '../dto/real-time/AdminHotelInfoDTO'
import {AdminInfo} from '../dto/real-time/AdminInfo'
import {adminDataResponse, GetAdminDataResponse} from '../dto/real-time/GetAdminDataResponse'
import {DetailRealTimeDTO} from '../dto/real-time/DetailRealTimeDTO'

@Injectable({
  providedIn: 'root'
})
export class RealTimeService {

  private gateUrl = 'http://localhost:55278/api';
  private adminUrl = '/admin/admin-info'

  constructor(private client: HttpClient) { 

  }

  getStatistics(): Observable<GetAdminDataResponse> {
    return this.client.get<GetAdminDataResponse>(this.gateUrl + this.adminUrl);
  }

  getTransports(): Observable<AdminTransportInfoDTO[]> {
    return of([
      {topNumber: 3, country: "Grecja", city: "Kos"},
      {topNumber: 1, country: "Grecja", city: "Rodos"},
      {topNumber: 2, country: "Grecja", city: "Ateny"}
    ])
  }

  getRoomTypes(): Observable<AdminRoomTypeInfoDTO[]> {
    return of([
      {topNumber: 3, typeOfRoom: "Cowabunga"},
      {topNumber: 1, typeOfRoom: "Funky Monkey"},
      {topNumber: 2, typeOfRoom: "Carribean Carramba"}
    ])
  }

  getHotels(): Observable<AdminHotelInfoDTO[]> {
    return of([
      {topNumber: 3, hotelName: "Hellenike suflakos"},
      {topNumber: 1, hotelName: "Mauro gyros"},
      {topNumber: 2, hotelName: "Apotheke agape"}
    ])
  }

  postUserInDetail(): Observable<void> {
    return this.client.post<void>("costam/api/plusUser", {});
  }

  postUserOutOfDetail(): Observable<void> {
    return this.client.post<void>("costam/api/minusUser", {});
  }

  getDetailRealTimeData(): Observable<DetailRealTimeDTO> {
    return this.client.get<DetailRealTimeDTO>("costam/api/detail");
  }
}
