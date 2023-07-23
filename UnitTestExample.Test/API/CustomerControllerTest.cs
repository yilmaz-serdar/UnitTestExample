using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UnitTestExample.API.Controllers;
using UnitTestExample.Data.Core;
using UnitTestExample.Data.Entities;
using UnitTestExample.Data.Fakers;
using Xunit;

namespace UnitTestExample.Test.API
{
    public class CustomerControllerTest
    {
        private readonly CustomerController customerController;
        private readonly Mock<ICustomerService> customerServiceMockObj;
        private readonly List<Customer> mockCustomers;

        public CustomerControllerTest()
        {
            customerServiceMockObj = new Mock<ICustomerService>();
            customerController = new CustomerController(customerServiceMockObj.Object);
            mockCustomers = new CustomerFaker().Generate(10);
        }

        [Theory]
        [InlineData(18)]
        [InlineData(38)]
        public void GetById_IdInvalid_ReturnNotFoundResult(int id)
        {
            Customer customer = null;
            customerServiceMockObj.Setup(x => x.GetById(id)).Returns(customer);

            var result = customerController.GetById(id);

            var actionResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public void GetById_IdValid_ReturnOkObjectResultWithCustomer()
        {
            Customer customer = mockCustomers.First();
            customerServiceMockObj.Setup(x => x.GetById(customer.Id)).Returns(customer);

            var result = customerController.GetById(customer.Id);

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var modelResult = Assert.IsType<Customer>(actionResult.Value);

            Assert.Equal(customer.Id, modelResult.Id);
            Assert.Equal(customer.Email, modelResult.Email);
        }

        [Fact]
        public void GetAll_ActionExecutes_ReturnOkObjectResultWithCustomers()
        {
            customerServiceMockObj.Setup(x => x.GetAll()).Returns(mockCustomers);

            var result = customerController.GetAll();

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var modelResult = Assert.IsType<List<Customer>>(actionResult.Value);

            Assert.Equal(mockCustomers.Count, modelResult.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnCreatedAtActionResultWithLocation()
        {
            var customer = mockCustomers.First();
            var result = customerController.Create(customer);

            customerServiceMockObj.Verify(x => x.Add(customer), Times.Once);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("GetById", actionResult.ActionName);
            Assert.Equal(201, actionResult.StatusCode);
        }

    }
}
