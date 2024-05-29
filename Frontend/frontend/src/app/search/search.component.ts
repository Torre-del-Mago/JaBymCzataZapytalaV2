import { Component } from '@angular/core';
import { BackendService } from '../backend/backend.service';
import { TripDTO } from '../dto/TripDTO';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent {
  destinations = ['', 'Grecja', 'Włochy', 'Hiszpania', 'Egipt'];
  startCities = ['', 'Warszawa', 'Gdańsk', 'Katowice'];
  destination = '';
  startCity = '';
  startDate = '';
  endDate = '';
  result: TripDTO[] = [];
  notAllValuesPresent = false;
  numberOfAdults: number = 1;
  numberOfChildren: number = 0;
  numberOfPeople: number = 0;
  canDecreaseChildren?: boolean = true;
  canDecreaseAdults?: boolean = false;
  canIncreasePeople?: boolean = false;


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
    this.result = this.service.getInfoForTrips(
      this.destination,
      this.startCity,
      new Date(this.startDate),
      new Date(this.endDate),
      this.numberOfAdults,
      this.numberOfChildren
    );
  }

  async detail(trip: TripDTO): Promise<void> {
    this.service.setCurrentTrip(trip);
    this.service.setNumbers(this.numberOfChildren, this.numberOfAdults)
    this.service.setDates(this.startDate, this.endDate)
    await this.router.navigateByUrl('detail');
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
