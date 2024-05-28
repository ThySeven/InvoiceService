using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceService.Controllers;
using InvoiceService.Models;
using InvoiceService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using InvoiceService.Repository;

namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceControllerTests
    {
        private Mock<IInvoiceRepository> _mockRepository;
        private Mock<ILogger<InvoiceController>> _mockLogger;
        private Mock<IConfiguration> _mockConfig;
        private InvoiceController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IInvoiceRepository>();
            _mockLogger = new Mock<ILogger<InvoiceController>>();
            _mockConfig = new Mock<IConfiguration>();
            _controller = new InvoiceController(_mockLogger.Object, _mockConfig.Object, _mockRepository.Object);
        }

        [TestMethod]
        public void CreateInvoice_ReturnsOkResult_WhenInvoiceIsCreated()
        {
            // Arrange
            var invoice = new InvoiceModel { Id = "bc77ece8-032d-4269-88a9-9d186358b885", Email = "test@example.com", Price = 100, Description = "Test invoice" };

            // Act
            var result = _controller.CreateInvoice(invoice);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(invoice, okResult.Value);
        }

        [TestMethod]
        public void CreateInvoice_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var invoice = new InvoiceModel { Id = "bc77ece8-032d-4269-88a9-9d186358b885", Email = "test@example.com", Price = 100, Description = "Test invoice" };
            _mockRepository.Setup(repo => repo.CreateInvoice(invoice)).Throws(new Exception("Error"));

            // Act
            var result = _controller.CreateInvoice(invoice);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to create invoice invoice"));
        }

        [TestMethod]
        public void GetInvoiceById_ReturnsOkResult_WithInvoice()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";
            var invoice = new InvoiceModel { Id = invoiceId, Email = "test@example.com", Price = 100, Description = "Test invoice" };
            _mockRepository.Setup(repo => repo.GetById(invoiceId)).Returns(invoice);

            // Act
            var result = _controller.GetInvoiceById(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(invoice, okResult.Value);
        }

        [TestMethod]
        public void GetInvoiceById_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";
            _mockRepository.Setup(repo => repo.GetById(invoiceId)).Throws(new Exception("Error"));

            // Act
            var result = _controller.GetInvoiceById(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to get by id"));
        }

        [TestMethod]
        public void GetAllInvoice_ReturnsOkResult_WithInvoices()
        {
            // Arrange
            var invoices = new List<InvoiceModel>
        {
            new InvoiceModel { Id = "bc77ece8-032d-4269-88a9-9d186358b885", Email = "test@example.com", Price = 100, Description = "Test invoice" },
            new InvoiceModel { Id = "bc77ece8-032d-4269-88a9-9d186358b886", Email = "test2@example.com", Price = 200, Description = "Test invoice 2" }
        };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(invoices);

            // Act
            var result = _controller.GetAllInvoice();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult; Assert.AreEqual(invoices, okResult.Value);
        }

        [TestMethod]
        public void GetAllInvoice_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).Throws(new Exception("Error"));

            // Act
            var result = _controller.GetAllInvoice();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to get"));
        }

        [TestMethod]
        public void ValidateInvoice_ReturnsOkResult_WhenInvoiceIsValidated()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";

            // Act
            var result = _controller.ValidateInvoice(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsTrue(((string)okResult.Value).Contains("Invoice validated with id"));
        }

        [TestMethod]
        public void ValidateInvoice_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";
            _mockRepository.Setup(repo => repo.ValidateInvoice(invoiceId)).Throws(new Exception("Error"));

            // Act
            var result = _controller.ValidateInvoice(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to validate Invoice"));
        }

        [TestMethod]
        public void DeleteInvoice_ReturnsOkResult_WhenInvoiceIsDeleted()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";

            // Act
            var result = _controller.DeleteInvoice(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsTrue(((string)okResult.Value).Contains("Invoice deleted with id"));
        }

        [TestMethod]
        public void DeleteInvoice_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var invoiceId = "bc77ece8-032d-4269-88a9-9d186358b885";
            _mockRepository.Setup(repo => repo.DeleteInvoice(invoiceId)).Throws(new Exception("Error"));

            // Act
            var result = _controller.DeleteInvoice(invoiceId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to delete id"));
        }

        [TestMethod]
        public async Task CreatePaymentLink_ReturnsOkResult_WithPaymentLink()
        {
            // Arrange
            var payment = new PaymentModel { Price = 100, CurrencyCode = "DKK", InvoiceNumber = "bc77ece8-032d-4269-88a9-9d186358b885", Reference = "bc77ece8-032d-4269-88a9-9d186358b885" };
            var paymentLink = "http://dummy.payment.link";
            _mockRepository.Setup(repo => repo.CreatePaymentLink(payment)).ReturnsAsync(paymentLink);

            // Act
            var result = await _controller.CreatePaymentLink(payment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsTrue(((string)okResult.Value).Contains("Payment link created"));
        }

        [TestMethod]
        public async Task CreatePaymentLink_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var payment = new PaymentModel { Price = 100, CurrencyCode = "DKK", InvoiceNumber = "bc77ece8-032d-4269-88a9-9d186358b885", Reference = "bc77ece8-032d-4269-88a9-9d186358b885" };
            _mockRepository.Setup(repo => repo.CreatePaymentLink(payment)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.CreatePaymentLink(payment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue(((string)badRequestResult.Value).Contains("Failed to send payment link"));
        }
    }
}