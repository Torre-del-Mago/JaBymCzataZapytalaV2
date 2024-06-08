import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable, of} from 'rxjs'
import {AdminTransportInfoDTO} from '../dto/real-time/AdminTransportInfoDTO'
import {AdminRoomTypeInfoDTO} from '../dto/real-time/AdminRoomTypeInfoDTO'
import {AdminHotelInfoDTO} from '../dto/real-time/AdminHotelInfoDTO'

@Injectable({
  providedIn: 'root'
})
export class RealTimeService {

  constructor(private client: HttpClient) { 

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
}
