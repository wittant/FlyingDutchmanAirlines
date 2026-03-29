using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public sealed class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

	public Customer(string name)
	{
		Name = name;
	}

	internal class CustomerEqualityComparer : EqualityComparer<Customer>
    {
        public override bool Equals(Customer? x, Customer? y)
        {
            if(x is null || y is null){return false;}
            return x.CustomerId == y.CustomerId && x.Name == y.Name;
        }

        public override int GetHashCode(Customer obj)
        {
            int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue /2);
            return (obj.CustomerId + obj.Name + randomNumber).GetHashCode();
        }
    }

	public override bool Equals(object? obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) return false;
		Customer customer = (Customer)obj;
        return this.CustomerId == customer.CustomerId && this.Name == customer.Name;
    }

    public override int GetHashCode()
    {
        int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue /2);
        return (this.CustomerId + this.Name + randomNumber).GetHashCode();
    }
	
    public static bool operator == (Customer x, Customer y)
    {
        CustomerEqualityComparer customerEqualityComparer = new CustomerEqualityComparer();
        return customerEqualityComparer.Equals(x, y);
    }

    public static bool operator != (Customer x, Customer y) => !(x == y);
}
