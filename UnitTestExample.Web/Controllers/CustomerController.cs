using Microsoft.AspNetCore.Mvc;
using UnitTestExample.Data.Core;
using UnitTestExample.Data.Entities;

namespace UnitTestExample.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public ActionResult Index()
        {
            var customers = _customerService.GetAll();

            return View(customers);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var customer = _customerService.GetById(id.Value);

            if (customer == null)
                return NotFound();


            return View(customer);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerService.Add(customer);
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }
    }
}
