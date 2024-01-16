using Crud.Controllers;
using Crud.Core.Models;
using Crud.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Crud.Tests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    [Test]
    public async Task Index_ShouldReturnViewWithCustomers()
    {
        var cancellationToken = new CancellationToken();
        var expectedCustomers = new List<Customer>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "1234567890",
                Subscribed = true
            }
        };

        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(service => service.GetAllCustomersAsync(cancellationToken)).ReturnsAsync(expectedCustomers);

        var controller = new HomeController(mockCustomerService.Object);

        var result = await controller.Index(cancellationToken) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Model, Is.EqualTo(expectedCustomers));
        });

    }

    [Test]
    public async Task Edit_Get_ShouldReturnViewWithCustomer()
    {
        var cancellationToken = new CancellationToken();
        var customerId = Guid.NewGuid();
        var expectedCustomer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Email = "johndoe@example.com",
            Phone = "1234567890",
            Subscribed = true
        };

        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(service => service.GetCustomer(customerId, cancellationToken)).ReturnsAsync(expectedCustomer);

        var controller = new HomeController(mockCustomerService.Object);

        var result = await controller.Edit(customerId, cancellationToken) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Model, Is.EqualTo(expectedCustomer));
        });
    }


    [Test]
    public async Task Delete_ShouldRedirectToIndex()
    {
        var cancellationToken = new CancellationToken();
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@example.com",
            Phone = "1234567890",
            Subscribed = true
        };

        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(service => service.DeleteCustomerAsync(customer, cancellationToken)).ReturnsAsync(true);

        var controller = new HomeController(mockCustomerService.Object);

        var result = await controller.Delete(customer, cancellationToken) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.ActionName, Is.EqualTo("Index"));
            Assert.That(result?.ControllerName, Is.EqualTo("Home"));
        });
    }

    [Test]
    public void Add_Get_ShouldReturnView()
    {
        var controller = new HomeController(Mock.Of<ICustomerService>());

        var result = controller.Add() as ViewResult;

        Assert.That(result, Is.Not.Null);
    }
}