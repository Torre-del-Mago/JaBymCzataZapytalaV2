import { OfferDTO } from '../model/OfferDTO';

export type ReserveOfferRequest = {
    registration: number;
    offer: OfferDTO;
}