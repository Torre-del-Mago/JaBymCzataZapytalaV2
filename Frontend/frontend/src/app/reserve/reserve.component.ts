import { Component } from '@angular/core';
import { TripDTO } from '../dto/model/TripDTO';
import { BackendService } from '../backend/backend.service';
import { PayResponse } from '../dto/response/PayResponse';
import { ReserveOfferResponse } from '../dto/response/ReserveOfferResponse';
import { Router } from '@angular/router';
import {map, Observable, of, tap} from 'rxjs';

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
  offerInfo?: ReserveOfferResponse 
  result$: Observable<string> = of('')
  timerRef?: Object
  paid = false
  waitingForPayment = false
  timeUp = false

  constructor(private service: BackendService, private router: Router ) {

  }

  ngOnInit() {
    if(this.service.getReservedOffer() === undefined || this.service.getUser() === '') {
      this.router.navigateByUrl('');
    }
    this.trip = this.service.getReservedOffer();
    let numbers = this.service.getNumbers();
    this.numberOfAdults = numbers[0];
    this.numberOfChildren = numbers[1];
    this.timerRef = this.timer(1);
    this.offerInfo = this.service.getOfferInfo();
  }

  timer(minute: number) {
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
        this.timeUp = true
        clearInterval(timer);
      }
    }, 1000);
    return timer;
  }

  async tryPaying() {
    this.waitingForPayment = true
    //TODO: Dodaj catchError()
    const offerPaidFor = 0;
    this.result$ = this.service.tryPaying(this.offerInfo!.offerId, this.trip!.price!).pipe(
      tap((r: PayResponse) => {
        if(!this.paid) {
          if(r.answer === offerPaidFor) {
            clearInterval(this.timerRef! as number)
            this.router.navigateByUrl('');
            this.paid = true
          }
          this.waitingForPayment = false
        }
      }),
      map((r: PayResponse) => { 
        return r.answer === offerPaidFor ? "Zapłacono za ofertę" : "Płatność się nie powiodła"}));
  }
}
