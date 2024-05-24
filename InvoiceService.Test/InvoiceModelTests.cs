using InvoiceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceModelTests
    {
        [TestMethod]
        public void Id_GivenValidGuid_ShouldSetValue()
        {
            // Arrange
            var invoice = new InvoiceModel();
            var newId = Guid.NewGuid().ToString();

            // Act
            invoice.Id = newId;

            // Assert
            Assert.AreEqual(newId, invoice.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid GUID format.")]
        public void Id_GivenInvalidGuid_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Id = "InvalidGuid";

            // Assert - Exception Expected
        }

        [TestMethod]
        public void PaidStatus_ShouldSetAndGet()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.PaidStatus = true;

            // Assert
            Assert.IsTrue(invoice.PaidStatus);
        }

        [TestMethod]
        public void Price_GivenValidValue_ShouldSetValue()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Price = 100.50;

            // Assert
            Assert.AreEqual(100.50, invoice.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Price cannot be negative.")]
        public void Price_GivenNegativeValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Price = -10;

            // Assert - Exception Expected
        }

        [TestMethod]
        public void Description_GivenValidValue_ShouldSetValue()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Description = "Test Description";

            // Assert
            Assert.AreEqual("Test Description", invoice.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Description cannot be empty or whitespace.")]
        public void Description_GivenEmptyValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Description = "";

            // Assert - Exception Expected
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Description cannot be empty or whitespace.")]
        public void Description_GivenWhitespaceValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Description = " ";

            // Assert - Exception Expected
        }

        [TestMethod]
        public void Address_GivenValidValue_ShouldSetValue()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Address = "123 Main St";

            // Assert
            Assert.AreEqual("123 Main St", invoice.Address);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Address cannot be empty or whitespace.")]
        public void Address_GivenEmptyValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Address = "";

            // Assert - Exception Expected
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Address cannot be empty or whitespace.")]
        public void Address_GivenWhitespaceValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Address = " ";

            // Assert - Exception Expected
        }

        [TestMethod]
        public void Email_GivenValidValue_ShouldSetValue()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Email = "test@example.com";

            // Assert
            Assert.AreEqual("test@example.com", invoice.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid email format.")]
        public void Email_GivenInvalidValue_ShouldThrowException()
        {
            // Arrange
            var invoice = new InvoiceModel();

            // Act
            invoice.Email = "invalid-email";

            // Assert - Exception Expected
        }

        [TestMethod]
        public void CreatedAt_ShouldBeInitializedProperly()
        {
            // Arrange
            var startDate = DateTime.Now;
            var invoice = new InvoiceModel();
            var endDate = DateTime.Now;

            // Act
            var createdAt = invoice.CreatedAt;

            // Assert
            Assert.IsTrue(createdAt >= startDate && createdAt <= endDate);
        }
    }
}
