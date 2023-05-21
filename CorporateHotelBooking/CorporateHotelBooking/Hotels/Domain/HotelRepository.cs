﻿namespace CorporateHotelBooking.Hotels.Domain;

public interface HotelRepository
{
    void AddHotel(int hotelId, string hotelName);
    Hotel GetById(int hotelId);
}