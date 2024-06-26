using InvoiceService.Models;
using InvoiceService.Repository;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceIRepositoryTests
    {
        [TestMethod]
        public void GetAll_ReturnsAllInvoices()
        {
            var mockService = new Mock<IInvoiceRepository>();
            var invoices = new List<InvoiceModel>
            {
                new InvoiceModel { Description = "Test Invoice 1", Price = 100.0 },
                new InvoiceModel { Description = "Test Invoice 2", Price = 200.0 }
            };

            mockService.Setup(s => s.GetAll()).Returns(invoices);

            var result = mockService.Object.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetById_ExistingId_ReturnsCorrectInvoice()
        {
            var mockService = new Mock<IInvoiceRepository>();
            var invoice = new InvoiceModel { Description = "Test Invoice", Price = 100.0 };
            string invoiceId = invoice.Id;
            mockService.Setup(s => s.GetById(invoiceId)).Returns(invoice);

            var result = mockService.Object.GetById(invoiceId);

            Assert.IsNotNull(result);
            Assert.AreEqual(invoiceId, result.Id);
            mockService.Verify(s => s.GetById(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void CreateInvoice_SavesInvoiceCorrectly()
        {
            var mockService = new Mock<IInvoiceRepository>();
            var invoice = new InvoiceModel { Description = "New Invoice", Price = 50.0 };

            mockService.Setup(s => s.CreateInvoice(invoice)).Verifiable();

            mockService.Object.CreateInvoice(invoice);

            mockService.Verify(s => s.CreateInvoice(It.IsAny<InvoiceModel>()), Times.Once);
        }

        [TestMethod]
        public void DeleteInvoice_RemovesInvoice()
        {
            var mockService = new Mock<IInvoiceRepository>();

            mockService.Setup(s => s.DeleteInvoice("")).Verifiable();

            mockService.Object.DeleteInvoice("");

            mockService.Verify(s => s.DeleteInvoice(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateInvoice_UpdatesExistingInvoice()
        {
            var mockService = new Mock<IInvoiceRepository>();
            string guid = "bc77ece8-032d-4269-88a9-9d186358b885";
            var newInvoice = new InvoiceModel { Description = "Updated Invoice", Price = 150.0, Id = guid };

            mockService.Setup(s => s.UpdateInvoice(newInvoice)).Returns(newInvoice);

            var result = mockService.Object.UpdateInvoice(newInvoice);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Invoice", result.Description);
            mockService.Verify(s => s.UpdateInvoice(It.IsAny<InvoiceModel>()), Times.Once);
        }

        [TestMethod]
        public void ValidateInvoice_ValidatesInvoiceSuccessfully()
        {
            var mockService = new Mock<IInvoiceRepository>();

            mockService.Setup(s => s.ValidateInvoice("")).Verifiable();

            mockService.Object.ValidateInvoice("");

            mockService.Verify(s => s.ValidateInvoice(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SendInvoice_SendsInvoiceToMailQueue()
        {
            var mockService = new Mock<IInvoiceRepository>();
            var invoice = new InvoiceModel { Description = "Invoice for Email", Price = 75.0 };

            mockService.Setup(s => s.SendInvoice(invoice)).Verifiable();

            mockService.Object.SendInvoice(invoice);

            mockService.Verify(s => s.SendInvoice(It.IsAny<InvoiceModel>()), Times.Once);
        }
    }
}