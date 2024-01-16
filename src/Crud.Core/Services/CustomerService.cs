using Crud.Core.Enums;
using Crud.Core.Models;
using Crud.Core.Repositories;
using Crud.Core.UnitOfWorks;

namespace Crud.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomersAsync(cancellationToken);
        }

        public async Task<Customer?> GetCustomer(Guid id, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomerByIdAsync(id, cancellationToken);
        }

        public async Task<Customer?> EditCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            var customerModel = await _customerRepository.GetCustomerByIdAsync(customer.Id, cancellationToken);
            if (customerModel is null) 
                return null;
            customerModel.Name = customer.Name;
            customerModel.Phone = customer.Phone;
            customerModel.Email = customer.Email;
            customerModel.Subscribed = customer.Subscribed;
            await _unitOfWork.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            var customerModel = new Customer()
            {
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Subscribed = customer.Subscribed
            };
            await _customerRepository.AddCustomerAsync(customerModel, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            var customerModel = await _customerRepository.GetCustomerByIdAsync(customer.Id, cancellationToken);
            if (customerModel is null) 
                return false;
            await _customerRepository.DeleteCustomerAsync(customerModel.Id, cancellationToken);
           var saveChangesAsync = await _unitOfWork.SaveChangesAsync();
           return saveChangesAsync != (int)DatabaseResponseCodes.SaveChangesFailed;
        }
    }
}
