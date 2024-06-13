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

export type LastTravelAgencyChangesDTO = {
    recentChanges: TravelAgencyEntryDTO[]
}

export type TravelAgencyEntryDTO = {
    eventName: string;
    idChanged: number;
    change: number;
};

export type GetAdminDataResponse = {
    topHotelsDto: TopHotelsDto;
    topRoomTypesDto: TopRoomTypesDto;
    topDepartureDto: TopDepartureDto;
    topDestinationDto: TopDestinationDto;
    lastTravelAgencyChangesDto: LastTravelAgencyChangesDTO;
};


