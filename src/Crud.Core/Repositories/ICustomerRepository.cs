using Crud.Core.Models;

namespace Crud.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersAsync(CancellationToken cancellationToken);
        Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddCustomerAsync(Customer? customer, CancellationToken cancellationToken);
        Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken);
    }
}
