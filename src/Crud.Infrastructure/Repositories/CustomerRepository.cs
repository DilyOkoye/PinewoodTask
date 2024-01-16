using Crud.Core.Models;
using Crud.Core.Repositories;
using Crud.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CrudContext _context;

        public CustomerRepository(CrudContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.FindAsync(id, cancellationToken);
        }

        public async Task AddCustomerAsync(Customer? customer, CancellationToken cancellationToken)
        {
            if (customer != null)
                await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public async Task DeleteCustomerAsync(Guid id, CancellationToken cancellationToken)
        {
            var customerToDelete = await _context.Customers.FindAsync(id, cancellationToken);
            if (customerToDelete != null) _context.Customers.Remove(customerToDelete);
        }   
    }
}
