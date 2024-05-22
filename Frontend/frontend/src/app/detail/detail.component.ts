import { Component, OnInit } from '@angular/core';
import { TripDTO } from '../dto/TripDTO';
import { RoomDTO } from '../dto/RoomDTO';
import { BackendService } from '../backend/backend.service';
import { ActivatedRoute } from '@angular/router';
import { FlightDTO } from '../dto/FlightDTO';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
})
export class DetailComponent implements OnInit {
  public trip?: TripDTO;
  public hotelName?: string;
  public country?: string;
  public destinationName?: string;
  public chosenFlight?: FlightDTO;
  public pricePerPerson: number = 0;
  public mealType?: string;
  public numberOfAdults: number = 0;
  public numberOfChildren: number = 0;
  public numberOfPeople: number = 0;
  public canDecreaseChildren?: boolean;
  public canDecreaseAdults?: boolean;
  public canIncreasePeople?: boolean;
  public roomName?: string;
  public mealTypes: string[] = [];
  public flights: FlightDTO[] = [];
  public configurationAvailable = false;
  public beginDate = ''
  public endDate = ''
  public numOfDays = 0
  private days = 0

  constructor(private route: ActivatedRoute, private service: BackendService) {}

  displayPrice() {
  }

  getFormattedDate(date: Date) {
    return date.toISOString().slice(0, 10);
  }

  changeDays() {
    let timediff = new Date(this.endDate).getTime() - new Date(this.beginDate).getTime();
    this.days = Math.round(timediff / (1000 * 3600 * 24));
  }

  ngOnInit() {
    this.trip = this.service.getCurrentTrip();
    this.hotelName = this.trip?.HotelName;
    this.destinationName = this.trip?.City;
    this.pricePerPerson = Math.random() * 500 + 500;
    this.numberOfAdults = this.service.getNumbers()[0];
    this.numberOfChildren = this.service.getNumbers()[1];
    let dates = this.service.getDates()
    this.beginDate = dates[0];
    this.endDate = dates[1];
    this.changeDays();
    this.numberOfPeople = this.numberOfChildren + this.numberOfAdults;
    this.mealTypes = this.trip?.TypesOfMeals || [];
    this.flights = this.trip?.PossibleFlights || [];
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = false;
    }
    if (this.numberOfAdults == 0) {
      this.canDecreaseChildren = false;
    }
    if (this.numberOfChildren == 0) {
      this.canDecreaseAdults = false;
    }
    this.mealType = this.trip?.TypesOfMeals[0];
  }

  addAdult(): void {
    this.numberOfAdults++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseAdults = false;
    this.pricePerPerson -= Math.random() * 40 + 40;
    this.displayPrice();
  }

  minusAdult(): void {
    this.numberOfAdults--;
    this.numberOfPeople--;
    if (this.numberOfAdults == 0) {
      this.canDecreaseAdults = true;
    }
    this.canIncreasePeople = false;
    this.pricePerPerson += Math.random() * 40 + 40;
    this.displayPrice();
  }

  addChild(): void {
    this.numberOfChildren++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseChildren = false;
    this.pricePerPerson -= Math.random() * 20 + 20;
    this.displayPrice();
  }

  minusChild(): void {
    this.numberOfChildren--;
    this.numberOfPeople--;
    if (this.numberOfChildren == 0) {
      this.canDecreaseChildren = true;
    }
    this.canIncreasePeople = false;
    this.pricePerPerson += Math.random() * 20 + 20;
    this.displayPrice();
  }

  getRoomsFor(numberOfPeople: number, numberOfRooms: number): RoomDTO[] {
    return this.trip?.Rooms.filter(r => r.Count >= numberOfRooms && r.NumberOfPeopleForTheRoom == numberOfPeople) || [];
  }

  flightChanged(flight: FlightDTO): void {
    console.log(flight)
    this.changePriceForFlight(this.trip!.ChosenFlight, flight);
    this.trip!.ChosenFlight = flight;
  }

  roomChanged(room: RoomDTO, position: number): void {
    this.changePriceForRoom(this.trip!.ChosenRooms![position], room, this.trip!.RoomCombination![position])
    this.trip!.ChosenRooms![position] = room;
  }

  changePriceForFlight(flight: FlightDTO, newflight: FlightDTO): void {
    this.trip!.Price! -= (flight.PricePerSeat * (this.numberOfAdults + this.numberOfChildren));
    this.trip!.Price! += (newflight.PricePerSeat * (this.numberOfAdults + this.numberOfChildren));
  }

  changePriceForRoom(room: RoomDTO, newroom: RoomDTO, numberOfRooms: number): void {
    this.trip!.Price! -= (room.PricePerRoom * this.days * numberOfRooms);
    this.trip!.Price! += (newroom.PricePerRoom * this.days * numberOfRooms);
  }

  compareRoomTypes(room: RoomDTO, nextRoom: RoomDTO): boolean {
    return room.TypeOfRoom == nextRoom.TypeOfRoom && room.NumberOfPeopleForTheRoom == nextRoom.NumberOfPeopleForTheRoom;
  }
}
