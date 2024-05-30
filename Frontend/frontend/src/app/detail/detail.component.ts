import { Component, OnInit } from '@angular/core';
import { TripDTO } from '../dto/TripDTO';
import { RoomDTO } from '../dto/RoomDTO';
import { ReserveOfferResponse } from '../dto/ReserveOfferResponse';
import { BackendService } from '../backend/backend.service';
import { ActivatedRoute } from '@angular/router';
import { FlightDTO } from '../dto/FlightDTO';
import { Subscription, of, Observable, pipe, map, catchError, tap } from 'rxjs';
import { Router } from '@angular/router';

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
  public configurationAvailable = true;
  public beginDate = '';
  public endDate = '';
  public numOfDays = 0;
  public backup: number[] = [0, 0]
  private days = 0;
  reserveError$: Observable<string> = of('');
  

  constructor(
    private route: ActivatedRoute,
    private service: BackendService,
    private router: Router
  ) {}

  displayPrice() {}

  getFormattedDate(date: Date) {
    return date.toISOString().slice(0, 10);
  }

  transformDate(notdate: Date): string {
    const date = new Date(notdate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const formattedDate = `${year}-${month}-${day}`;
    return formattedDate;
  }

  changeDays() {
    let timediff =
      new Date(this.trip!.endDate).getTime() - new Date(this.trip!.beginDate).getTime();
    this.days = Math.round(timediff / (1000 * 3600 * 24));
  }

  ngOnInit() {
    this.refreshTrip();
    this.pricePerPerson = Math.random() * 500 + 500;
    this.numberOfAdults = this.service.getNumbers()[0];
    this.numberOfChildren = this.service.getNumbers()[1];
    let dates = this.service.getDates();
    this.beginDate = dates[0];
    this.endDate = dates[1];
    this.changeDays();
    this.numberOfPeople = this.numberOfChildren + this.numberOfAdults;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = false;
    }
    if (this.numberOfAdults == 0) {
      this.canDecreaseChildren = false;
    }
    if (this.numberOfChildren == 0) {
      this.canDecreaseAdults = false;
    }
  }

  createBackup() {
    this.backup[0] = this.numberOfAdults;
    this.backup[1] = this.numberOfChildren;
  }

  addAdult(): void {
    this.createBackup();
    this.numberOfAdults++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseAdults = false;
    this.refreshTrip();
    this.pricePerPerson -= Math.random() * 40 + 40;
    this.displayPrice();
  }

  minusAdult(): void {
    this.createBackup();
    this.numberOfAdults--;
    this.numberOfPeople--;
    if (this.numberOfAdults == 0) {
      this.canDecreaseAdults = true;
    }
    this.canIncreasePeople = false;
    this.refreshTrip();
    this.pricePerPerson += Math.random() * 40 + 40;
    this.displayPrice();
  }

  addChild(): void {
    this.createBackup();
    this.numberOfChildren++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseChildren = false;
    this.refreshTrip();
    this.pricePerPerson -= Math.random() * 20 + 20;
    this.displayPrice();
  }

  minusChild(): void {
    this.createBackup()
    this.numberOfChildren--;
    this.numberOfPeople--;
    if (this.numberOfChildren == 0) {
      this.canDecreaseChildren = true;
    }
    this.canIncreasePeople = false;
    this.refreshTrip();
    this.pricePerPerson += Math.random() * 20 + 20;
    this.displayPrice();
  }

  getRoomsFor(numberOfPeople: number, numberOfRooms: number): RoomDTO[] {
    return (
      this.trip?.rooms.filter(
        (r: RoomDTO) =>
          r.count >= numberOfRooms &&
          r.numberOfPeopleForTheRoom == numberOfPeople
      ) || []
    );
  }

  flightChanged(flight: FlightDTO): void {
    console.log(flight);
    this.changePriceForFlight(this.trip!.chosenFlight, flight);
    this.trip!.chosenFlight = flight;
  }

  restore(): void {
    this.numberOfAdults = this.backup[0]
    this.numberOfChildren = this.backup[1]
  }

  roomChanged(room: RoomDTO, position: number): void {
    this.changePriceForRoom(
      this.trip!.chosenRooms![position],
      room,
      this.trip!.roomCombination![position]
    );
    this.trip!.chosenRooms![position] = room;
  }

  changePriceForFlight(flight: FlightDTO, newflight: FlightDTO): void {
    this.trip!.price! -=
      flight.pricePerSeat * (this.numberOfAdults + this.numberOfChildren);
    this.trip!.price! +=
      newflight.pricePerSeat * (this.numberOfAdults + this.numberOfChildren);
  }

  changePriceForRoom(
    room: RoomDTO,
    newroom: RoomDTO,
    numberOfRooms: number
  ): void {
    this.trip!.price! -= room.pricePerRoom * this.days * numberOfRooms;
    this.trip!.price! += newroom.pricePerRoom * this.days * numberOfRooms;
  }

  compareRoomTypes(room: RoomDTO, nextRoom: RoomDTO): boolean {
    return (
      room.typeOfRoom == nextRoom.typeOfRoom &&
      room.numberOfPeopleForTheRoom == nextRoom.numberOfPeopleForTheRoom
    );
  }

  async reserve(): Promise<void> {
    this.trip!.chosenMeal = this.mealType;
    this.reserveError$ = this.service.reserveOffer(
      this.trip!,
      this.numberOfAdults,
      this.numberOfChildren
    ).pipe(tap((result: ReserveOfferResponse) => {
      this.service.setOfferInfo(result);
      this.goToPayment()
    }), map((result: ReserveOfferResponse) => {
      return result.answer == 0 ? "Rezerwacja się nie powiodła. Przyczyna: " + result.error : '';
    }),
    catchError((err: any) => {return "Rezerwacja się nie powiodła"}));
  }

  async goToPayment() {
    await this.router.navigateByUrl('reserve');
  }

  private refreshTrip(): void {
    if (this.trip) {
      const tripsForChangedCriteria = this.service.getInfoForTrip(
        this.country ?? '',
        this.trip.chosenFlight.departure,
        new Date(this.beginDate),
        new Date(this.endDate),
        this.numberOfAdults,
        this.numberOfChildren,
        this.hotelName
      );
      if(tripsForChangedCriteria == null) {
        this.configurationAvailable = false;
        return;
      }
      this.trip = tripsForChangedCriteria;
      this.service.setCurrentTrip(this.trip!);
    } else {
      this.trip = this.service.getCurrentTrip();
    }
    this.country = this.trip?.country;
    this.hotelName = this.trip?.hotelName;
    this.destinationName = this.trip?.city;
    this.mealTypes = this.trip?.typesOfMeals || [];
    this.flights = this.trip?.possibleFlights || [];
    this.mealType = this.trip?.typesOfMeals[0];
    this.trip!.chosenFlight = this.trip?.possibleFlights.find((f: FlightDTO) => f.departure == this.trip?.chosenFlight.departure) || this.trip!.chosenFlight
    console.log(this.trip?.chosenFlight)
  }
}
