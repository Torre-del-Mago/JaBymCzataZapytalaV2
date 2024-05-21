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
  adults = 1;
  children = 0;
  result: TripDTO[] = [];
  notAllValuesPresent = false;

  constructor(private service: BackendService, private router: Router) {}

  search(): void {
    if(this.destination === '' || this.startCity === '' || this.endDate === '' || this.startDate === '' || (this.adults+this.children) === 0)
    {
      this.notAllValuesPresent = true;
    }
    this.result = this.service.getInfoForTrips(
      this.destination,
      this.startCity,
      new Date(this.startDate),
      new Date(this.endDate),
      this.adults,
      this.children
    );
  }

  async detail(trip: TripDTO): Promise<void> {
    this.service.setCurrentTrip(trip);
    this.service.setNumbers(this.children, this.adults)
    await this.router.navigateByUrl('detail');
  }
}
