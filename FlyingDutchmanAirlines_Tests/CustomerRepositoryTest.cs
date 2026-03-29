using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;


[TestClass]
public sealed class CustomerRepositoryTest
{
    private FlyingDutchmanAirlinesContext? _context;
    private CustomerRepository? _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
		var a = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>();
		var dbContextOptions = a.UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        Customer testCustomer = new Customer("David Neumann");
        _context.Customers.Add(testCustomer);
        await _context.SaveChangesAsync();

        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        bool result = await _repository!.CreateCustomer("David Neumann");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_DatabaseAccessError()
    {
        CustomerRepository customerRepository = new CustomerRepository(null!);
        Assert.IsNotNull(customerRepository);

        bool result = await customerRepository.CreateCustomer("David Neumann");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsNull()
    {
        bool result = await _repository!.CreateCustomer(null!);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsEmpty()
    {
        bool result = await _repository!.CreateCustomer(string.Empty);
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('#')]
    [DataRow('&')]
    [DataRow('$')]
    [DataRow('*')]
    [DataRow('%')]
    public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter)
    {
        bool result = await _repository!.CreateCustomer("David Neuman" + invalidCharacter);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetCustomerByName_Success()
    {
        Customer customer = await _repository!.GetCustomerByName("David Neumann");
        Assert.IsNotNull(customer);

        Customer dbCustomer = _context!.Customers.First();
        Assert.AreEqual<Customer>(customer, dbCustomer);
    }

    [TestMethod]
    [DataRow("#")]
    [DataRow("&")]
    [DataRow(null)]
    [DataRow("$")]
    [DataRow("*")]
    [DataRow("%")]
    [DataRow("")]
    public async Task GetCustomerByName_Failure_InvalidName(string invalidName)
    {
        await Assert.ThrowsAsync<CustomerNotFoundException>(() => _repository!.GetCustomerByName(invalidName));
    }
}
