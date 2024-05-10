using InvoiceService.Models;
using InvoiceService.Repository;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using InvoiceService.Repositories;
using MongoDB.Driver;

namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceMongoDBRepositoryTests
    {
        private Mock<IMongoCollection<InvoiceModel>> mockCollection;
        private InvoiceRepository repository; // This should be the actual class being tested.
        private Mock<IAsyncCursor<InvoiceModel>> mockCursor;
        private InvoiceModel testingInvoice = new();


        [TestInitialize]
        public void SetUp()
        {
            var mockClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockCollection = new Mock<IMongoCollection<InvoiceModel>>();
            mockCursor = new Mock<IAsyncCursor<InvoiceModel>>();

            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
            mockDatabase.Setup(d => d.GetCollection<InvoiceModel>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

            repository = new InvoiceRepository(mockDatabase.Object);  // Assuming the constructor takes a MongoClient and a string.
        }


        [TestMethod]
        public void CreateInvoice_InsertsInvoice()
        {
            repository.CreateInvoice(testingInvoice);
            mockCollection.Verify(c => c.InsertOne(It.IsAny<InvoiceModel>(), null, default(CancellationToken)), Times.Once);
        }

        [TestMethod]
        public void DeleteInvoice_DeletesById()
        {

            repository.DeleteInvoice(testingInvoice.Id);

            mockCollection.Verify(c => c.DeleteOne(It.IsAny<FilterDefinition<InvoiceModel>>(), default), Times.Once);
        }

        [TestMethod]
        public void GetAll_ReturnsAllInvoices()
        {
            var invoices = new List<InvoiceModel> { new InvoiceModel(), new InvoiceModel() };
            mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>())).Returns(true).Returns(false);
            mockCursor.Setup(_ => _.Current).Returns(invoices);
            mockCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<FindOptions<InvoiceModel, InvoiceModel>>(), default)).Returns(mockCursor.Object);

            var result = repository.GetAll();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetById_FindsInvoiceById()
        {
            var invoice = new InvoiceModel {};
            string invoiceId = invoice.Id;
            SetUpCursor(new List<InvoiceModel> { invoice });

            var result = repository.GetById(invoiceId);

            Assert.AreEqual(invoiceId, result.Id);
        }

        [TestMethod]
        public void UpdateInvoice_UpdatesInvoice()
        {
            var invoiceToUpdate = new InvoiceModel() { Price = 200 };
            SetUpCursor(new List<InvoiceModel> { invoiceToUpdate });

            var result = repository.UpdateInvoice(invoiceToUpdate);

            mockCollection.Verify(c => c.UpdateOne(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<UpdateDefinition<InvoiceModel>>(), It.IsAny<UpdateOptions>(), default), Times.Once);
            Assert.AreEqual(invoiceToUpdate.Price, result.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ValidateInvoice_ThrowsWhenNotFound()
        {
            mockCollection.Setup(x => x.UpdateOne(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<UpdateDefinition<InvoiceModel>>(), It.IsAny<UpdateOptions>(), default))
                          .Returns(new UpdateResult.Acknowledged(0, 0, null));

            repository.ValidateInvoice(testingInvoice.Id);
        }

        private void SetUpCursor(List<InvoiceModel> data)
        {
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                      .Returns(true)
                      .Returns(false);
            mockCursor.Setup(c => c.Current).Returns(data);
            mockCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<FindOptions<InvoiceModel, InvoiceModel>>(), default))
                          .Returns(mockCursor.Object);
        }
    }
}