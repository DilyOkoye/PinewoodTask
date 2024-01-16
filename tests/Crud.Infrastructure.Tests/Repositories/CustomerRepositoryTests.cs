using Crud.Core.Models;
using Crud.Infrastructure.DbContext;
using Crud.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Tests.Repositories;

[TestFixture]
public class CustomerRepositoryTests
{
    private static CrudContext CreateDbContext()
    {
        
        var options = new DbContextOptionsBuilder<CrudContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
        var dbContext = new CrudContext(options);
        return dbContext;
    }
    
    
    [Test]
    public async Task GetCustomersAsync_ShouldReturnListOfCustomers()
    {
        var dbContext = CreateDbContext();
        
        await dbContext.Customers.AddAsync(new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Dili",
            Email = "test@test.com",
            Subscribed = true,
            Phone = "0123456"
        });

        await dbContext.Customers.AddAsync(new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Dili2",
            Email = "test1@test.com",
            Subscribed = false,
            Phone = "01234565"
        });

        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);
        
        var result = await sut.GetCustomersAsync(CancellationToken.None);

        Assert.That(result, Has.Count.GreaterThan(1));
    }

    [Test]
    public async Task GetCustomersAsync_ShouldReturnEmptyList_When_NoCustomersExist()
    {
        var dbContext = CreateDbContext();

        var sut = new CustomerRepository(dbContext);

        var result = await sut.GetCustomersAsync(CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomerById()
    {
        var dbContext = CreateDbContext();

        var customer = new Customer
        {
            Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            Name = "Dili",
            Email = "test@test.com",
            Subscribed = true,
            Phone = "0123456"
        };
        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);

        var result = await sut.GetCustomerByIdAsync(customer.Id,CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(customer.Id));
        });
    }

    [Test]
    public async Task AddCustomerAsync_ShouldAddCustomerToContext()
    {
        var dbContext = CreateDbContext();
        var sut = new CustomerRepository(dbContext);
        var customer = new Customer
        {
            Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            Name = "Dili",
            Email = "test@test.com",
            Subscribed = true,
            Phone = "0123456"
        };

        await sut.AddCustomerAsync(customer,CancellationToken.None);

        Assert.That(dbContext.Entry(customer).State, Is.EqualTo(EntityState.Added));
        await dbContext.DisposeAsync();
    }

    [Test]
    public async Task DeleteCustomerAsync_ShouldRemoveCustomerFromContext()
    {
        var dbContext = CreateDbContext();

        var customer = new Customer
        {
            Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            Name = "Dili",
            Email = "test@test.com",
            Subscribed = true,
            Phone = "0123456"
        };
        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        var sut = new CustomerRepository(dbContext);

        await sut.DeleteCustomerAsync(customer.Id, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var result = await sut.GetCustomerByIdAsync(customer.Id, CancellationToken.None);

        Assert.That(result, Is.Null);

    }
}