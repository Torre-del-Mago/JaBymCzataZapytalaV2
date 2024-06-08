export type EntryDto = {
    name: string;
    numberOfElements: number;
};

export type TopDepartureDto = {
    topDepartures: EntryDto[];
};

export type TopDestinationDto = {
    topDestinations: EntryDto[];
};

export type TopHotelsDto = {
    topHotels: EntryDto[];
};

export type TopRoomTypesDto = {
    topRoomTypes: EntryDto[];
};

export type GetAdminDataResponse = {
    topHotelsDto: TopHotelsDto;
    topRoomTypesDto: TopRoomTypesDto;
    topDepartureDto: TopDepartureDto;
    topDestinationDto: TopDestinationDto;
};

export const adminDataResponse: GetAdminDataResponse = {
    topHotelsDto: {
        topHotels: [
            { name: "Hilton", numberOfElements: 5 },
            { name: "Marriott", numberOfElements: 3 }
        ]
    },
    topRoomTypesDto: {
        topRoomTypes: [
            { name: "Deluxe", numberOfElements: 10 },
            { name: "Suite", numberOfElements: 7 }
        ]
    },
    topDepartureDto: {
        topDepartures: [
            { name: "New York", numberOfElements: 15 },
            { name: "Los Angeles", numberOfElements: 12 }
        ]
    },
    topDestinationDto: {
        topDestinations: [
            { name: "Paris", numberOfElements: 20 },
            { name: "Tokyo", numberOfElements: 18 }
        ]
    }
};