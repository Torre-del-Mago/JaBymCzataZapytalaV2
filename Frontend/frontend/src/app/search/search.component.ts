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

  constructor(private service: BackendService, private router: Router) {}

  search(): void {
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
    await this.router.navigateByUrl('detail');
  }
}
