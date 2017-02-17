using HelloMvc.Controllers;
using HelloMvc.Models;
using HelloMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HelloMvc.Test
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Should_Return_View_with_Model() 
        {
            // Arrange
            var repoMock = new Mock<ICustomersRepository>();
            var customer = new Customer { FirstName = "Jack", LastName = "Doe" };
            repoMock.Setup(repo => repo.GetCustomer()).Returns(customer);
            var controller = new HomeController(repoMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var model = ((ViewResult)result).Model as Customer;
            Assert.Equal(customer, model);
        }
    }
}
