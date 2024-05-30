import { OfferDTO } from './OfferDTO';

export type ReserveOfferRequest = {
    registration: number;
    offer: OfferDTO;
}