import { Component } from '@angular/core';
import { RealTimeService } from '../real-time/real-time.service';
import { Observable, pipe, map, timer, flatMap } from 'rxjs';
import { AdminTransportInfoDTO } from '../dto/real-time/AdminTransportInfoDTO';
import { AdminHotelInfoDTO } from '../dto/real-time/AdminHotelInfoDTO';
import { AdminRoomTypeInfoDTO } from '../dto/real-time/AdminRoomTypeInfoDTO';
import { AdminInfo } from '../dto/real-time/AdminInfo';
import { realTimeObservable } from '../functions';
import { GetAdminDataResponse } from '../dto/real-time/GetAdminDataResponse';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
})
export class AdminPanelComponent {
  statistics?: Observable<GetAdminDataResponse>;

  constructor(private service: RealTimeService, private client: HttpClient) {}

  ngOnInit() {
    this.statistics = timer(0, 5000).pipe(
        flatMap((_) => {
            return this.service.getStatistics(this.client)
        })
    )
  }
}
