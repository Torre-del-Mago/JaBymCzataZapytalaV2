import { Component, OnInit } from '@angular/core';
import { TripDTO } from '../dto/TripDTO';
import { BackendService } from '../backend/backend.service';
import { ActivatedRoute } from '@angular/router';
import {FlightDTO} from '../dto/FlightDTO'
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
})
export class DetailComponent implements OnInit {

  public trip?: TripDTO
  public hotelName?: string
  public country?: string
  public destinationName?: string
  public chosenFlight?: FlightDTO
  public pricePerPerson: number = 0
  public priceRounded: string = ''
  public mealType?: string
  public numberOfAdults: number = 0
  public numberOfChildren: number = 0
  public numberOfPeople: number = 0
  public maxPeople?: number
  public canDecreaseChildren?: boolean
  public canDecreaseAdults?: boolean
  public canIncreasePeople?: boolean
  public roomName?: string
  public mealTypes: string[] = []
  public flights: FlightDTO[] = []
  public configurationNotAvailable = false

  constructor(private route: ActivatedRoute, private service: BackendService) {

  }

  displayPrice() {
    this.priceRounded = this.pricePerPerson.toFixed(2)
  }

  ngOnInit() {
    this.trip = this.service.getCurrentTrip();
    this.hotelName = this.trip?.HotelName
    this.destinationName = this.trip?.City
    this.pricePerPerson = Math.random() * (500) + 500;
    this.priceRounded = this.pricePerPerson.toFixed(2)
    this.numberOfAdults = this.service.getNumbers()[0]
    this.numberOfChildren = this.service.getNumbers()[1]
    this.numberOfPeople = this.numberOfChildren + this.numberOfAdults
    this.mealTypes = this.trip?.TypesOfMeals || [];
    this.flights = this.trip?.PossibleFlights || [];
    if(this.numberOfPeople == 6) {
      this.canIncreasePeople = false
    }
    if(this.numberOfAdults == 0) {
      this.canDecreaseChildren = false
    }
    if(this.numberOfChildren == 0) {
      this.canDecreaseAdults = false
    }
    this.mealType = this.trip?.TypesOfMeals[0]
  }

  addAdult() : void{
    this.numberOfAdults++
    this.numberOfPeople++
    if(this.numberOfPeople == 6) {
      this.canIncreasePeople = true
    }
    this.pricePerPerson -= Math.random() *40 + 40;
    this.displayPrice()
  }

  minusAdult() : void{
    this.numberOfAdults--;
    this.numberOfPeople--;
    if(this.numberOfAdults == 0) {
      this.canDecreaseAdults = true
    }
    this.pricePerPerson += Math.random() * 40 + 40;
    this.displayPrice()
  }

  addChild() : void{
    this.numberOfChildren++;
    this.numberOfPeople++;
    if(this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.pricePerPerson -= Math.random() * 20 + 20;
    this.displayPrice()
  }

  minusChild(): void {
    this.numberOfChildren--;
    this.numberOfPeople--;
    if(this.numberOfChildren == 0) {
      this.canDecreaseChildren = true;
    } 
    this.pricePerPerson += Math.random() * 20 + 20;
    this.displayPrice()
  }

  flightChanged(flight: FlightDTO): void {

  }
}
