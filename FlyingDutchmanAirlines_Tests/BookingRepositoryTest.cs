using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class BookingRepositoryTest
{
    private FlyingDutchmanAirlinesContext? _context;
    private BookingRepository? _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
        _repository = new BookingRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        await _repository!.CreateBooking(1, 0);
        Booking booking = _context!.Bookings.First();
        Assert.IsNotNull(booking);
        Assert.AreEqual(1, booking.CustomerId);
        Assert.AreEqual(0, booking.FlightNumber);
    }

    [TestMethod]
    [DataRow(-1, 1)]    
    [DataRow(1, -1)]
    [DataRow(-1, -1)]
    public async Task CreateBooking_Failed_InvalidInput(int customerId, int flightId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _repository!.CreateBooking(customerId, flightId));
    }
}
