import { Component } from '@angular/core';
import { TripDTO } from '../dto/TripDTO';
import { BackendService } from '../backend/backend.service';

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.css']
})
export class ReserveComponent {
  trip?: TripDTO
  numberOfAdults: number = 0;
  numberOfChildren: number = 0;


  constructor(private service: BackendService ) {

  }

  ngOnInit() {
    this.trip = this.service.getReservedOffer();
    let numbers = this.service.getNumbers();
    this.numberOfAdults = numbers[0];
    this.numberOfChildren = numbers[1];
  }

}
