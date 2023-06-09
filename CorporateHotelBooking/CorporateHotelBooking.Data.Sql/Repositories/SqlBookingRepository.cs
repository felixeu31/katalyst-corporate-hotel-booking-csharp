﻿using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.Mappers;

namespace CorporateHotelBooking.Data.Sql.Repositories;

public class SqlBookingRepository : IBookingRepository
{
    private readonly CorporateHotelDbContext _context;

    public SqlBookingRepository(CorporateHotelDbContext context)
    {
        _context = context;
    }


    public void Add(Booking booking)
    {
        var bookingData = BookingDataMapper.MapBookingDataFrom(booking);

        _context.Bookings.Add(bookingData);

        _context.SaveChanges();
    }

    public Booking? Get(BookingId bookingId)
    {
        var bookingData = _context.Bookings.FirstOrDefault(x => x.BookingId == bookingId.Value);

        var booking = BookingDataMapper.HydrateDomainFrom(bookingData);

        return booking;
    }

    public IEnumerable<Booking> GetBookingsBy(HotelId hotelId)
    {
        var bookingDatas = _context.Bookings.Where(x => x.HotelId.Equals(hotelId.Value)).ToList();

        var bookings = bookingDatas.Select(BookingDataMapper.HydrateDomainFrom);

        return bookings;
    }

    public void DeleteEmployeeBookings(EmployeeId employeeId)
    {
        foreach (var employeeBooking in _context.Bookings.Where(x => x.BookedBy.Equals(employeeId.Value)).ToList())
        {
            _context.Bookings.Remove(employeeBooking);
        }

        _context.SaveChanges();
    }
}