using Crud.Core.Models;
using Crud.Core.Repositories;
using Crud.Core.Services;
using Crud.Core.UnitOfWorks;
using Moq;

namespace Crud.Core.Tests.Services;

[TestFixture]
public class CustomerServiceTests
{
    [Test]
    public async Task GetAllCustomersAsync_ShouldReturnListOfCustomers()
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

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        mockCustomerRepository.Setup(repo => repo.GetCustomersAsync(cancellationToken)).ReturnsAsync(expectedCustomers);

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.GetAllCustomersAsync(cancellationToken);

        Assert.That(result, Has.Count.EqualTo(expectedCustomers.Count));
    }

    [Test]
    public async Task GetCustomer_ShouldReturnCustomerById()
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

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId, cancellationToken)).ReturnsAsync(expectedCustomer);

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.GetCustomer(customerId, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(customerId));
        });
    }

    [Test]
    public async Task EditCustomerAsync_ShouldReturnEditedCustomer()
    {
        var cancellationToken = new CancellationToken();
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer
        {
            Id = customerId,
            Name = "ExistingCustomer",
            Phone = "123-456-7890",
            Email = "existing@example.com",
            Subscribed = true
        };

        var editedCustomer = new Customer
        {
            Id = customerId,
            Name = "EditedCustomer",
            Phone = "987-654-3210",
            Email = "edited@example.com",
            Subscribed = false
        };

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId, cancellationToken)).ReturnsAsync(existingCustomer);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(1); 

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.EditCustomerAsync(editedCustomer, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Name, Is.EqualTo(editedCustomer.Name));
            Assert.That(result?.Phone, Is.EqualTo(editedCustomer.Phone));
            Assert.That(result?.Email, Is.EqualTo(editedCustomer.Email));
            Assert.That(result?.Subscribed, Is.EqualTo(editedCustomer.Subscribed));
        });
    }

    [Test]
    public async Task EditCustomerAsync_ShouldReturnNullIfCustomerNotFound()
    {
        var cancellationToken = new CancellationToken();
        var customerId = Guid.NewGuid();

        var editedCustomer = new Customer
        {
            Id = customerId,
            Name = "EditedCustomer",
            Phone = "987-654-3210",
            Email = "edited@example.com",
            Subscribed = false
        };

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId, cancellationToken)).ReturnsAsync((Customer)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.EditCustomerAsync(editedCustomer, cancellationToken);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddCustomerAsync_ShouldReturnAddedCustomer()
    {
        var cancellationToken = new CancellationToken();
        var newCustomer = new Customer
        {
            Name = "NewCustomer",
            Phone = "123-456-7890",
            Email = "new@example.com",
            Subscribed = true
        };

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(1); // Assuming successful save

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.AddCustomerAsync(newCustomer, cancellationToken);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Name, Is.EqualTo(newCustomer.Name));
            Assert.That(result?.Phone, Is.EqualTo(newCustomer.Phone));
            Assert.That(result?.Email, Is.EqualTo(newCustomer.Email));
            Assert.That(result?.Subscribed, Is.EqualTo(newCustomer.Subscribed));
        });
    }

    [Test]
    public async Task DeleteCustomerAsync_ShouldReturnTrueOnSuccessfulDelete()
    {
        var cancellationToken = new CancellationToken();
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer
        {
            Id = customerId,
            Name = "ExistingCustomer",
            Phone = "123-456-7890",
            Email = "existing@example.com",
            Subscribed = true
        };

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId, cancellationToken)).ReturnsAsync(existingCustomer);
        mockCustomerRepository.Setup(repo => repo.DeleteCustomerAsync(customerId, cancellationToken)).Returns(Task.CompletedTask);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(1); // Assuming successful save

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);

        var result = await customerService.DeleteCustomerAsync(existingCustomer, cancellationToken);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteCustomerAsync_ShouldReturnFalseIfCustomerNotFound()
    {
        var cancellationToken = new CancellationToken();
        var customerId = Guid.NewGuid();

        var mockCustomerRepository = new Mock<ICustomerRepository>();
        _ = mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId, cancellationToken)).ReturnsAsync((Customer)null);

        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var customerService = new CustomerService(mockUnitOfWork.Object, mockCustomerRepository.Object);
        
        var result = await customerService.DeleteCustomerAsync(new Customer { Id = customerId }, cancellationToken);

        Assert.That(result, Is.False);
    }
}