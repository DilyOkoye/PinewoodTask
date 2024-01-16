using Crud.Core.Models;

namespace Crud.Core.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken);

        Task<Customer?> GetCustomer(Guid id, CancellationToken cancellationToken);

        Task<Customer?> EditCustomerAsync(Customer customer, CancellationToken cancellationToken);

        Task<Customer?> AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerAsync(Customer customer, CancellationToken cancellationToken);
    }
}
