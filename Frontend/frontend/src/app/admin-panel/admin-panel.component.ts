import { Component } from '@angular/core';
import {RealTimeService} from '../real-time/real-time.service'
import {Observable, pipe, map} from 'rxjs'
import {AdminTransportInfoDTO} from '../dto/real-time/AdminTransportInfoDTO'
import {AdminHotelInfoDTO} from '../dto/real-time/AdminHotelInfoDTO'
import {AdminRoomTypeInfoDTO} from '../dto/real-time/AdminRoomTypeInfoDTO'
import {AdminInfo} from '../dto/real-time/AdminInfo'

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent {

  transports?: Observable<AdminTransportInfoDTO[]>
  roomTypes?: Observable<AdminRoomTypeInfoDTO[]>
  hotels?: Observable<AdminHotelInfoDTO[]>

  constructor(private service: RealTimeService) {

  }

  private sortLambda = (a: AdminInfo, b: AdminInfo) => {
      return a.topNumber < b.topNumber ? -1 : 1
    }

  ngOnInit() {
    this.transports = this.service.getTransports().pipe(map(r => {
      return r.sort(this.sortLambda)
    }))
    this.roomTypes = this.service.getRoomTypes().pipe(map(r => {
      return r.sort(this.sortLambda)
    }))
    this.hotels = this.service.getHotels().pipe(map(r => {
      return r.sort(this.sortLambda)
    }))
  }

}
