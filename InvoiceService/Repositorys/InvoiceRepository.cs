﻿using InvoiceService.Models;
using InvoiceService.Repository;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
namespace InvoiceService.Repositorys
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMongoCollection<InvoiceModel> _invoices;
        private string _connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
        IAuctionCoreQueue queue;

        public InvoiceRepository()
        {
            queue = new RabbitMQueue();
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("AuctionCoreServices");
            _invoices = database.GetCollection<InvoiceModel>("Invoices");
        }
        public InvoiceRepository(IMongoDatabase db)
        {
            _invoices = db.GetCollection<InvoiceModel>("Invoices");
        }


        public void CreateInvoice(InvoiceModel invoice)
        {
            _invoices.InsertOne(invoice);
            queue.Add(new AutoMail
            {
                Model = new InvoiceModel(),
                ReceiverMail = "heh"
            });
        }

        public string CreatePaymentLink(PaymentModel payment)
        {
            //Seends Payment link to payment provider

            throw new NotImplementedException();
        }

        public void DeleteInvoice(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id); // Assuming "Id" is a property of InvoiceModel and its type is int
            _invoices.DeleteOne(filter);
        }

        public IEnumerable<InvoiceModel> GetAll()
        {
            return _invoices.Find(invoice => true).ToList();
        }

        public InvoiceModel GetById(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id);
            return _invoices.Find(filter).SingleOrDefault();
        }

        public void SendInvoice(InvoiceModel invoice)
        {
            // Simulate sending the invoice via a messaging system or an email service.
            // This operation would typically involve another service and not directly relate to MongoDB actions.
            throw new NotImplementedException();
        }

        public void SendParcelInformation(ParcelModel parcel)
        {
            //Sends parcel information to parcel provider

            throw new NotImplementedException();
        }

        public InvoiceModel UpdateInvoice(InvoiceModel newInvoiceData)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", newInvoiceData.Id);
            var update = Builders<InvoiceModel>.Update
                            .Set(x => x.Price, newInvoiceData.Price) // Example of updating the amount
                                                                       // Add other properties to update as required
                            ;
            _invoices.UpdateOne(filter, update);
            return newInvoiceData;
        }

        public void ValidateInvoice(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id); // Assumption: 'Id' is the property representing the unique identifier.
            var update = Builders<InvoiceModel>.Update.Set(i => i.PaidStatus, true); // Assuming 'PaidStatus' is the property to update.

            var result = _invoices.UpdateOne(filter, update);

            // Optionally, check the result to see if the update was successful
            if (result.MatchedCount == 0)
            {
                // Handle the case where no document was found with the provided id.
                // For example, you could throw a NotFoundException or handle it as you see fit.
                throw new KeyNotFoundException($"No invoice found with ID {id}.");
            }
            else if (result.ModifiedCount == 0)
            {
                // Handle the case where the document was found, but not modified,
                // which could happen if the invoice was already marked as paid.
                // This might not require an action, but you can log this situation or handle it as needed.
            }
            // If needed, you can also return some information (like a boolean indicating success or the updated document), 
            // depending on whether the method's return type is void in your original design.
        }
    }
}