using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Domain.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "John Doe";
            var email = "john.doe@example.com";

            // Act
            var customer = new Customer(id, name, email);

            // Assert
            Assert.Equal(id, customer.Id);
            Assert.Equal(name, customer.Name);
            Assert.Equal(email, customer.Email);
        }
    }
}