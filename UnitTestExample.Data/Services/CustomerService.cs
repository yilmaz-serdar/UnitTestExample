using System.Collections.Generic;
using System.Linq;
using UnitTestExample.Data.Core;
using UnitTestExample.Data.Entities;
using UnitTestExample.Data.Fakers;

namespace UnitTestExample.Data.Services
{
    public class CustomerService: ICustomerService
    {
        private static List<Customer> _customers;
        public CustomerService()
        {
            _customers = new CustomerFaker().Generate(25);
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(x=> x.Id == id);
        }

        public List<Customer> GetAll()
        {
            return _customers;
        }

        public void Add(Customer customer)
        {
            customer.Id = _customers.Max(x => x.Id) + 1;
            _customers.Add(customer);
        }
    }
}
