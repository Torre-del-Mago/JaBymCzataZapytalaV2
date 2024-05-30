import { Component } from '@angular/core';
import { TripDTO } from '../dto/TripDTO';
import { BackendService } from '../backend/backend.service';
import { PayResponse } from '../dto/PayResponse';
import { ReserveOfferResponse } from '../dto/ReserveOfferResponse';
import { Router } from '@angular/router';
import {map, Observable, of} from 'rxjs';

@Component({
  selector: 'app-reserve',
  templateUrl: './reserve.component.html',
  styleUrls: ['./reserve.component.css']
})
export class ReserveComponent {
  trip?: TripDTO
  numberOfAdults: number = 0;
  numberOfChildren: number = 0;
  display?: string
  canPay = true
  offerInfo?: ReserveOfferResponse 
  result$: Observable<string> = of('')

  constructor(private service: BackendService, private router: Router ) {

  }

  ngOnInit() {
    this.trip = this.service.getReservedOffer();
    let numbers = this.service.getNumbers();
    this.numberOfAdults = numbers[0];
    this.numberOfChildren = numbers[1];
    this.timer(1);
    this.offerInfo = this.service.getOfferInfo();
  }

  timer(minute: number): void {
    // let minute = 1;
    let seconds: number = minute * 60;
    let textSec: any = "0";
    let statSec: number = 60;

    const prefix = minute < 10 ? "0" : "";

    const timer = setInterval(() => {
      seconds--;
      if (statSec != 0) statSec--;
      else statSec = 59;

      if (statSec < 10) {
        textSec = "0" + statSec;
      } else textSec = statSec;

      this.display = `${prefix}${Math.floor(seconds / 60)}:${textSec}`;

      if (seconds == 0) {
        this.canPay = false
        clearInterval(timer);
        this.router.navigateByUrl('');
      }
    }, 1000);
  }

  async tryPaying() {
    //TODO: Dodaj catchError()
    this.result$ = this.service.tryPaying(this.offerInfo!.offerId, this.trip!.price!).pipe(
      map((r: PayResponse) => { return r.answer === 1 ? "Zapłacono za ofertę" : "Płatność się nie powiodła"}));
  }
}
