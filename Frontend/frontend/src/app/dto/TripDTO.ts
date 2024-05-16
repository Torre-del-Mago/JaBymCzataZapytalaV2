export type TripDTO {
    HotelName: string,
    Country: string, 
    City: string,
    BeginDate: Date,
    EndDate: Date,
    TypesOfMeals: string[],
    Discount: number,
    Rooms: RoomDTO[],
    ChosenFlight: FlightDTO,
    PossibleFlights: FlightDTO[]
}