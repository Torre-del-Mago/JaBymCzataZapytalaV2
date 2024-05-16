import { Component } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
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

  search(): void {
  } 
}
