using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;
    public BookingRepository(FlyingDutchmanAirlinesContext _context) => this._context = _context;

    public async Task CreateBooking(int customerId, int flightNumber)
    {
        if(customerId < 0 || flightNumber < 0) 
        { 
            Console.WriteLine($"ArgumentException in CreateBooking: CustomerId={customerId}, FlightNumber={flightNumber}");
            throw new ArgumentException("Invalid input: CustomerId and FlightNumber must be non-negative integers."); 
        }
        try
        {
            Booking newBooking = new Booking{ CustomerId = customerId, FlightNumber = flightNumber };
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during database query: {ex.Message}");
            throw new CouldNotAddBookingToDatabaseException();
        }
    }
}