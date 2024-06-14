import { Component, OnInit } from '@angular/core';
import { TripDTO } from '../dto/model/TripDTO';
import { RoomDTO } from '../dto/model/RoomDTO';
import { GenerateTripResponse } from '../dto/response/GenerateTripResponse';
import { ReserveOfferResponse } from '../dto/response/ReserveOfferResponse';
import { BackendService } from '../backend/backend.service';
import { RealTimeService } from '../real-time/real-time.service';
import { ActivatedRoute } from '@angular/router';
import { FlightDTO } from '../dto/model/FlightDTO';
import { Subscription, of, Observable, pipe, map, catchError, tap, firstValueFrom, flatMap, timer, shareReplay } from 'rxjs';
import { Router } from '@angular/router';
import {HotelStatisticsInfoResponse} from '../dto/real-time/DetailRealTimeDTO'
import {realTimeObservable} from '../functions'
import { HttpClient } from '@angular/common/http';

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
  private subscriptionIn?: Subscription
  private subscriptionOut?: Subscription
  public detailInfo?: Subscription
  public detailInfoText?: HotelStatisticsInfoResponse
  numberOfToddlers: number = 0;
  numberOfTeenagers: number = 0;
  numberOfNewborns: number = 0;
  canDecreaseToddlers?: boolean = true;
  canDecreaseTeenagers?: boolean = true;
  canDecreaseNewborns?: boolean = true;
  

  constructor(
    private route: ActivatedRoute,
    private service: BackendService,
    private router: Router,
    private realTimeService: RealTimeService,
    private client: HttpClient
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
    if(this.service.getCurrentTrip() === undefined || this.service.getUser() === '') {
      this.router.navigateByUrl('');
    }
    this.getTrip();
    this.subscriptionIn = this.realTimeService.postUserInDetail(this.trip!.hotelId).subscribe();
    this.startListening();
    this.detailInfo = timer(0, 5000).pipe(
        flatMap((_) => {
            return this.realTimeService.getDetailRealTimeData(this.client, this.trip!.hotelId)
        })
    ).subscribe(x => this.detailInfoText = x)
    this.pricePerPerson = Math.random() * 500 + 500;
    this.numberOfAdults = this.service.getNumbers()[0];
    this.numberOfChildren = this.service.getNumbers()[1];
    this.numberOfToddlers = this.service.getNumbers()[2];
    this.numberOfTeenagers = this.service.getNumbers()[3];
    this.numberOfNewborns = this.service.getNumbers()[4];
    let dates = this.service.getDates();
    this.beginDate = dates[0];
    this.endDate = dates[1];
    this.changeDays();
    this.numberOfPeople = this.numberOfChildren + this.numberOfAdults;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = false;
    }
    this.canDecreaseAdults = this.numberOfAdults == 0;
    this.canDecreaseNewborns = this.numberOfNewborns == 0;
    this.canDecreaseToddlers = this.numberOfToddlers == 0;
    this.canDecreaseTeenagers = this.numberOfTeenagers == 0;
  }

  private startListening(): void {
    window.addEventListener("beforeunload", this.ngOnDestroy.bind(this));
  }
 
  private stopListening(): void {
    window.removeEventListener("beforeunload", this.ngOnDestroy.bind(this));
  }

  ngOnDestroy() {
    this.subscriptionOut = this.realTimeService.postUserOutOfDetail(this.trip!.hotelId).subscribe();
    this.stopListening();
    this.detailInfo?.unsubscribe()
  }

  createBackup() {
    this.backup[0] = this.numberOfAdults;
    this.backup[1] = this.numberOfChildren;
  }

  async addAdult(): Promise<void> {
    this.createBackup();
    this.numberOfAdults++;
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    this.canDecreaseAdults = false;
    await this.startChange()
  }

  async minusAdult(): Promise<void> {
    this.createBackup();
    this.numberOfAdults--;
    this.numberOfPeople--;
    if (this.numberOfAdults == 0) {
      this.canDecreaseAdults = true;
    }
    this.canIncreasePeople = false;
    await this.startChange()
  }

  async addChild(which_child: string): Promise<void> {
    this.createBackup();
    this.numberOfChildren++;
    if(which_child === "teenager") {
      this.numberOfTeenagers++;
      this.canDecreaseTeenagers = false;
    }
    else if(which_child === "toddler") {
      this.numberOfToddlers++;
      this.canDecreaseToddlers = false;
    }
    else if(which_child === "newborn") {
      this.numberOfNewborns++;
      this.canDecreaseNewborns = false;
    }
    this.numberOfPeople++;
    if (this.numberOfPeople == 6) {
      this.canIncreasePeople = true;
    }
    await this.startChange()
  }

  async minusChild(which_child: string): Promise<void> {
    this.createBackup()
    this.numberOfChildren--;
    this.numberOfPeople--;
    if(which_child === "teenager") {
      this.numberOfTeenagers--;
      if (this.numberOfTeenagers == 0) {
        this.canDecreaseTeenagers = true;
      }
    }
    else if(which_child === "toddler") {
      this.numberOfToddlers--;
      if (this.numberOfNewborns == 0) {
        this.canDecreaseNewborns = true;
      }
    }
    else if(which_child === "newborn") {
      this.numberOfNewborns--;
      if (this.numberOfChildren == 0) {
        this.canDecreaseNewborns = true;
      }
    }
    this.canIncreasePeople = false;
    await this.startChange()
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

  async startChange(): Promise<void> {
    await this.refreshTrip().then((value: GenerateTripResponse) => {
      console.log(value)
      let tripsForChangedCriteria = this.service.changeTrips([value.tripDTO], this.beginDate, this.endDate, this.numberOfAdults, this.numberOfChildren);
      if(tripsForChangedCriteria == null) {
        this.configurationAvailable = false;
        return;
      }
      this.trip = tripsForChangedCriteria[0];
      this.service.setCurrentTrip(this.trip!);
      this.setTrips();
      }).catch((error: any) => {
        console.log(error);
        this.configurationAvailable = false;
      });;
  }

  async reserve(): Promise<void> {
    console.log("Started reserving")
    let offerReserved = 0;
    this.trip!.chosenMeal = this.mealType;
    this.reserveError$ = this.service.reserveOffer(
      this.trip!,
      this.numberOfAdults,
      this.numberOfChildren
    ).pipe(tap((result: ReserveOfferResponse) => {
      if(result.answer === offerReserved) {
        this.service.setOfferInfo(result);
        this.goToPayment()
      }
    }), map((result: ReserveOfferResponse) => {
      return result.answer !== offerReserved ? "Rezerwacja się nie powiodła. Przyczyna: " + result.error : '';
    }),
    catchError((err: any) => {return "Rezerwacja się nie powiodła"}));
  }

  async goToPayment() {
    await this.router.navigateByUrl('reserve');
  }

  private async refreshTrip(): Promise<GenerateTripResponse> {
    const value = await firstValueFrom(this.service.getInfoForTripNew(this.trip!.city, this.trip!.country, this.trip!.chosenFlight.departure, this.trip!.beginDate, this.trip!.endDate,
      this.numberOfAdults, this.numberOfChildren, this.trip!.hotelId)
    )
    console.log(value)
    return value
  }

  private getTrip(): void {
    this.trip = this.service.getCurrentTrip();
    this.setTrips();
  }

  setTrips() {

      this.country = this.trip?.country;

      this.hotelName = this.trip?.hotelName;

      this.destinationName = this.trip?.city;

      this.mealTypes = this.trip?.typesOfMeals || [];

      this.flights = this.trip?.possibleFlights || [];

      this.mealType = this.trip?.typesOfMeals[0];

      console.log(this.trip!.chosenFlight)
      console.log(this.trip!.possibleFlights)
      this.trip!.possibleFlights.push(this.trip!.chosenFlight)
      let flight = this.trip!.possibleFlights.find((f: FlightDTO) => f.departure == this.trip!.chosenFlight.departure)
      if(flight !== undefined) {
        this.trip!.chosenFlight = flight
        console.log("Found in possible flights")
      }
      console.log(this.trip!.chosenFlight);
      this.displayPrice();
  }
  
}
