using Bogus;
using UnitTestExample.Data.Entities;

namespace UnitTestExample.Data.Fakers
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            RuleFor(o => o.Id, f => f.Random.Number(1, 10000));
            RuleFor(o => o.FirstName, f => f.Person.FirstName);
            RuleFor(o => o.LastName, f => f.Person.LastName);
            RuleFor(o => o.Email, f => f.Person.Email);
            RuleFor(o => o.Avatar, f => f.Person.Avatar);
            RuleFor(o => o.Job, f => f.Name.JobTitle());
        }

    }
}
