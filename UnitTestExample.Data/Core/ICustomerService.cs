using System.Collections.Generic;
using UnitTestExample.Data.Entities;

namespace UnitTestExample.Data.Core
{
    public interface ICustomerService
    {
        Customer GetById(int id);
        List<Customer> GetAll();
        void Add(Customer customer);
    }
}
