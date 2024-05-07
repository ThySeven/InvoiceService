using InvoiceService.Models;
using InvoiceService.Repository;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using InvoiceService.Repositorys;
using MongoDB.Driver;

namespace InvoiceService.Test
{
    [TestClass]
    public class InvoiceMongoDBRepositoryTests
    {
        private Mock<IMongoCollection<InvoiceModel>> mockCollection;
        private IInvoiceRepository repository;
        private Mock<IAsyncCursor<InvoiceModel>> mockCursor;

        [TestInitialize]
        public void SetUp()
        {
            var mockClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockCollection = new Mock<IMongoCollection<InvoiceModel>>();
            mockCursor = new Mock<IAsyncCursor<InvoiceModel>>();

            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
            mockDatabase.Setup(d => d.GetCollection<InvoiceModel>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

            Environment.SetEnvironmentVariable("MongoDBConnectionString", "localhost:27018");
            repository = new InvoiceIRepository();  // No change in instantiation
        }


        [TestMethod]
        public void CreateInvoice_InsertsInvoice()
        {
            var invoice = new InvoiceModel();

            repository.CreateInvoice(invoice);

            mockCollection.Verify(c => c.InsertOne(invoice, null, default), Times.Once);
        }

        [TestMethod]
        public void DeleteInvoice_DeletesById()
        {
            int idToRemove = 1;

            repository.DeleteInvoice(idToRemove);

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
            int invoiceId = 1;
            var invoice = new InvoiceModel { Id = invoiceId };
            SetUpCursor(new List<InvoiceModel> { invoice });

            var result = repository.GetById(invoiceId);

            Assert.AreEqual(invoiceId, result.Id);
        }

        [TestMethod]
        public void UpdateInvoice_UpdatesInvoice()
        {
            var invoiceToUpdate = new InvoiceModel() { Id = 1, Price = 200 };
            SetUpCursor(new List<InvoiceModel> { invoiceToUpdate });

            var result = repository.UpdateInvoice(invoiceToUpdate);

            mockCollection.Verify(c => c.UpdateOne(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<UpdateDefinition<InvoiceModel>>(), It.IsAny<UpdateOptions>(), default), Times.Once);
            Assert.AreEqual(invoiceToUpdate.Price, result.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ValidateInvoice_ThrowsWhenNotFound()
        {
            int invoiceId = 1;
            mockCollection.Setup(x => x.UpdateOne(It.IsAny<FilterDefinition<InvoiceModel>>(), It.IsAny<UpdateDefinition<InvoiceModel>>(), It.IsAny<UpdateOptions>(), default))
                          .Returns(new UpdateResult.Acknowledged(0, 0, null));

            repository.ValidateInvoice(invoiceId);
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