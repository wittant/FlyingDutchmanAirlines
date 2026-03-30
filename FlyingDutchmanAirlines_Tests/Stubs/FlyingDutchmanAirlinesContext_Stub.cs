using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlyingDutchmanAirlines_Tests.Stubs;

public class FlyingDutchmanAirlinesContext_Stub : FlyingDutchmanAirlinesContext
{    
    public FlyingDutchmanAirlinesContext_Stub(DbContextOptions<FlyingDutchmanAirlinesContext> options): base(options)
    {
        base.Database.EnsureDeleted();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        IEnumerable<Booking> bookings = pendingChanges.Select(e => e.Entity).OfType<Booking>();

        if(bookings.Any(b => b.CustomerId != 1))
        {
            throw new Exception("Simulated database error: Invalid CustomerId in Booking.");
        }
       
        await base.SaveChangesAsync(cancellationToken);
        return 1;
    }
}