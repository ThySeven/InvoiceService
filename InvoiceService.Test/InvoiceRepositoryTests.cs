using InvoiceService.Models;
using InvoiceService.Repositories;
using InvoiceService.Services;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceRepositoryQueueTests
    {
        private Mock<IMongoCollection<InvoiceModel>> _mockCollection;
        private Mock<IAuctionCoreQueue> _mockQueue;
        private Mock<IMongoDatabase> _mockDatabase;
        private InvoiceRepository _invoiceRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize mocks
            _mockCollection = new Mock<IMongoCollection<InvoiceModel>>();
            _mockQueue = new Mock<IAuctionCoreQueue>();
            _mockDatabase = new Mock<IMongoDatabase>();

            // Setup the mock to return the mocked collection
            _mockDatabase
                .Setup(db => db.GetCollection<InvoiceModel>("Invoices", null))
                .Returns(_mockCollection.Object);

            // Initialize the repository with the mocked database and queue
            _invoiceRepository = new InvoiceRepository(_mockDatabase.Object)
            {
                queue = _mockQueue.Object
                
            };
        }

        [TestMethod]
        public async Task CreateInvoice_ShouldAddInvoiceAndSendMail()
        {
            // Arrange
            var invoice = new InvoiceModel { Id = "bc77ece8-032d-4269-88a9-9d186358b885", Email = "test@example.com", Price = 100 };
            _mockQueue.Setup(q => q.Add(It.IsAny<MailModel>()));

            // Act
            await _invoiceRepository.CreateInvoice(invoice);

            // Assert
            _mockCollection.Verify(c => c.InsertOne(invoice, null, default), Times.Once);
            _mockQueue.Verify(q => q.Add(It.Is<MailModel>(
                m => m.ReceiverMail == "test@example.com" && m.Header == "GrønOgOlsen Invoice" && !string.IsNullOrEmpty(m.Content)
            )), Times.Once);
        }
    }
}
