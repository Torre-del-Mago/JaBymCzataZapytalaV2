import { Component } from '@angular/core';
import { BackendService } from '../backend/backend.service';
import { TripDTO } from '../dto/TripDTO';
import { Router } from '@angular/router';
import {AsyncPipe} from '@angular/common'
import {Subscription, of, Observable, pipe, tap, map, catchError} from 'rxjs'; 

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  providers: [
    AsyncPipe
  ]
})
export class SearchComponent {
  destinations = ['', 'grecja', 'brazylia', 'cypr', 'egipt'];
  startCities = ['','bydgoszcz', 'gdansk', 'katowice', 'krakow', 'lublin', 'lodz', 'olsztyn-mazury',
  'poznan', 'rzeszow', 'szczecin', 'warszawa', 'warszawa-radom', 'wroclaw',
  'zielona-gora'];
  destination = '';
  startCity = '';
  startDate = '';
  endDate = '';
  result$: Observable<TripDTO[]> = of([]);
  notAllValuesPresent = false;
  numberOfAdults: number = 1;
  numberOfChildren: number = 0;
  numberOfPeople: number = 0;
  canDecreaseChildren?: boolean = true;
  canDecreaseAdults?: boolean = false;
  canIncreasePeople?: boolean = false;
  noMatchesFound: boolean = false;

  subscription: Subscription = new Subscription();


  constructor(private service: BackendService, private router: Router) {}

  search(): void {
    if(this.destination === '' || this.startCity === '' || this.endDate === '' || this.startDate === '' || (this.numberOfAdults+this.numberOfChildren) === 0)
    {
      this.notAllValuesPresent = true;
    }
    else {
      this.notAllValuesPresent = false;
    }
    console.log(this.startDate)

    this.result$ = this.service.getInfoForTrips(
      this.destination,
      this.startCity,
      this.startDate,
      this.endDate,
      this.numberOfAdults,
      this.numberOfChildren
    ).pipe(tap((t: TripDTO[]) => {console.log(t); this.noMatchesFound = false}), map((t: TripDTO[]) => {
      return this.service.changeTrips(t, this.startDate, this.endDate, this.numberOfAdults, this.numberOfChildren)
    }), catchError((err:any) => {this.noMatchesFound = true; return of([])}))
/*
    this.result = this.service.getInfoForTrips(
      this.destination,
      this.startCity,
      new Date(this.startDate),
      new Date(this.endDate),
      this.numberOfAdults,
      this.numberOfChildren
    );
    */
  }

  async detail(trip: TripDTO): Promise<void> {
    if(this.service.getUser() === '')
    {
      alert("Żeby móc przejrzeć ofertę należy być zalogowanym")
    }
    else {
      this.service.setCurrentTrip(trip);
      this.service.setNumbers(this.numberOfChildren, this.numberOfAdults)
      this.service.setDates(this.startDate, this.endDate)
      await this.router.navigateByUrl('detail');
    }
  }

  
  transformDate(notdate: Date): string {
    const date = new Date(notdate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const formattedDate = `${year}-${month}-${day}`;
    return formattedDate;
  }

  addAdult(): void {
    this.numberOfAdults++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseAdults = false;
  }

  minusAdult(): void {
    this.numberOfAdults--;
    this.numberOfPeople--;
    if (this.numberOfAdults == 0) {
      this.canDecreaseAdults = true;
    }
    this.canIncreasePeople = false;
  }

  addChild(): void {
    this.numberOfChildren++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseChildren = false;
  }
  
  
  minusChild(): void {
    this.numberOfChildren--;
    this.numberOfPeople--;
    if (this.numberOfChildren == 0) {
      this.canDecreaseChildren = true;
    }
    this.canIncreasePeople = false;
  }
}
