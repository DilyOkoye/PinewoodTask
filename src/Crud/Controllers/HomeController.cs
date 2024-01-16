using Microsoft.AspNetCore.Mvc;
using Crud.Core.Models;
using Crud.Core.Services;

namespace Crud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;

        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllCustomersAsync(cancellationToken);
            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
        {
            var customers =await _customerService.GetCustomer(id, cancellationToken);
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer, CancellationToken cancellationToken)
        {
            await _customerService.EditCustomerAsync(customer, cancellationToken);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Customer customer, CancellationToken cancellationToken)
        {
            await _customerService.DeleteCustomerAsync(customer, cancellationToken);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Customer customer, CancellationToken cancellationToken)
        {
            await _customerService.AddCustomerAsync(customer, cancellationToken);
            return View();
        }

    }
}
