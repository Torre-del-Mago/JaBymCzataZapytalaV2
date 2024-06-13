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

  getStatistics(client: HttpClient): Observable<GetAdminDataResponse> {
    return client.get<GetAdminDataResponse>('http://localhost:55278/api/admin/admin-info');
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

  postUserInDetail(hotelId: number): Observable<void> {
    return this.client.post<void>("http://localhost:55278/api/hotelStatistics/addNewWatchingClient?hotelId="+hotelId,{});
  }

  postUserOutOfDetail(hotelId: number): Observable<void> {
    return this.client.post<void>("http://localhost:55278/api/hotelStatistics/removeWatchingClient?hotelId="+hotelId, {});
  }

  getDetailRealTimeData(client: HttpClient, hotelId: number): Observable<DetailRealTimeDTO> {
    return client.get<DetailRealTimeDTO>("http://localhost:55278/api/hotelStatistics/getInfo?hotelId="+hotelId);
  }
}
