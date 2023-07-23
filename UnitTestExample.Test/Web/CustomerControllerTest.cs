using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UnitTestExample.Data.Core;
using UnitTestExample.Data.Entities;
using UnitTestExample.Data.Fakers;
using UnitTestExample.Web.Controllers;
using Xunit;

namespace UnitTestExample.Test.Web
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

        [Fact]
        public void Index_ActionExecutes_ReturnViewWithCustomers()
        {
            customerServiceMockObj.Setup(x => x.GetAll()).Returns(mockCustomers);

            var result = customerController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<List<Customer>>(viewResult.Model);

            Assert.Equal<int>(mockCustomers.Count, modelResult.Count);
        }

        [Fact]
        public void Details_IdParameterIsNull_ReturnRedirectToIndexAction()
        {
            var result = customerController.Details(null);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Details_IdInvalid_ReturnNotFound()
        {
            Customer customer = null;
            customerServiceMockObj.Setup(x => x.GetById(It.IsAny<int>())).Returns(customer);

            var result = customerController.Details(It.IsAny<int>());

            var viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public void Details_IdValid_ReturnViewWithCustomer()
        {
            var customer = mockCustomers.First();

            customerServiceMockObj.Setup(x => x.GetById(customer.Id)).Returns(customer);

            var result = customerController.Details(customer.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<Customer>(viewResult.Model);

            Assert.Equal(customer.Email, modelResult.Email);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnView()
        {
            var result = customerController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InValidModelState_ReturnView()
        {
            customerController.ModelState.AddModelError("Email", "Email Required");

            var customer = mockCustomers.First();
            var result = customerController.Create(customer);

            customerServiceMockObj.Verify(x => x.Add(It.IsAny<Customer>()), Times.Never);

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<Customer>(viewResult.Model);

            Assert.Equal(customer.FirstName, modelResult.FirstName);
        }

        [Fact]
        public void Create_ValidModelState_ReturnRedirectToIndexAction()
        {
            Customer customerParam = null;
            customerServiceMockObj.Setup(x => x.Add(It.IsAny<Customer>()))
                .Callback<Customer>(customer => customerParam = customer);

            var customer = mockCustomers.First();
            var result = customerController.Create(customer);

            customerServiceMockObj.Verify(x => x.Add(It.IsAny<Customer>()), Times.Once);

            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionResult.ActionName);
            Assert.Equal(customer.Email, customerParam.Email);
        }
    }
}
