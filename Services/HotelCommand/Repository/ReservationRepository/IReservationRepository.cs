﻿using HotelCommand.Database.Tables;

namespace HotelCommand.Repository.ReservationRepository
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task<Reservation> GetReservationByOfferIdAsync(int offerId);

        Task<List<Reservation>> GetReservationByHotelIdDatesAndNotDeleted(int hotelId, DateOnly beginDate,
            DateOnly endDate);
        void InsertReservation(Reservation reservation);
    }
}