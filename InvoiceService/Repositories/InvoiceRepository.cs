using InvoiceService.Models;
using InvoiceService.Repository;
using InvoiceService.Services;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
namespace InvoiceService.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMongoCollection<InvoiceModel> _invoices;
        private string _connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
        IAuctionCoreQueue queue;

        MailModel mail;

        public InvoiceRepository()
        {
            queue = new RabbitMQueue();
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("AuctionCoreServices");
            _invoices = database.GetCollection<InvoiceModel>("Invoices");

            mail = new MailModel()
            {
                Header = "GrønOgOlsen Invoice - Betal!",
                Content = (new InvoiceHtmlModel(new InvoiceModel { Email = "customer@example.com" })).HtmlContent,
                ReceiverMail = "customer@example.com"
            };
        }
        public InvoiceRepository(IMongoDatabase db)
        {
            _invoices = db.GetCollection<InvoiceModel>("Invoices");
        }


        public void CreateInvoice(InvoiceModel invoice)
        {
            _invoices.InsertOne(invoice);
            queue.Add(new MailModel
            {
                ReceiverMail = invoice.Email,
                Content = (new InvoiceHtmlModel(invoice)).HtmlContent,
                Header = mail.Header
            });
        }

        public string CreatePaymentLink(PaymentModel payment)
        {
            try
            {
                WebManager.GetInstance.HttpClient.PostAsJsonAsync("https://thisisyourdummypaymentlinknotintegratedtopaypal.yet/payment/submit", payment);
            }
            catch(Exception ex)
            {
                AuctionCoreLogger.Logger.Error("This error is intended, it shows a dummy http call to create a PayPal payment link");
            }
            Task.Delay(2000);
            
            return $"https://thisisyourdummypaymentlinknotintegratedtopaypal.yet/payment/{payment.Reference}";
        }

        public void DeleteInvoice(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id);
            _invoices.DeleteOne(filter);
        }

        public IEnumerable<InvoiceModel> GetAll()
        {
            return _invoices.Find(invoice => true).ToList();
        }

        public InvoiceModel GetById(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id);
            return _invoices.Find(filter).FirstOrDefault();
        }

        public void SendInvoice(InvoiceModel invoice)
        {
            try
            {
                MailModel mail = new MailModel()
                {
                    ReceiverMail = invoice.Email,
                    Content = (new InvoiceHtmlModel(invoice)).HtmlContent,
                    Header = this.mail.Header
                };
                queue.Add(mail);
            }
            catch(Exception ex)
            {
                AuctionCoreLogger.Logger.Fatal($"Failed to add invoice mail to mailQueue # {ex}");
            }
        }

        public string SendParcelInformation(ParcelModel parcel)
        {
            try
            {
                WebManager.GetInstance.HttpClient.PostAsJsonAsync("https://thisisyourdummyparcelnotintegratedtogls.yet/shipment/submit", parcel);
            }
            catch (Exception ex)
            {
                AuctionCoreLogger.Logger.Error("This error is intended, it shows a dummy http call to create a GLS parcel delivery");
            }
            Task.Delay(2000);

            return $"https://thisisyourdummyparcelnotintegratedtogls.yet/shipment/{parcel.Reference}";
        }

        public InvoiceModel UpdateInvoice(InvoiceModel newInvoiceData)
        {
            InvoiceModel currentInvoice = GetById(newInvoiceData.Id);

            Console.WriteLine(JsonSerializer.Serialize(currentInvoice));

            if (newInvoiceData.PaidStatus == default)
                newInvoiceData.PaidStatus = currentInvoice.PaidStatus;
            if (newInvoiceData.Price == default)
                newInvoiceData.Price = currentInvoice.Price;
            if (string.IsNullOrEmpty(newInvoiceData.Description))
                newInvoiceData.Description = currentInvoice.Description;
            if (string.IsNullOrEmpty(newInvoiceData.Address))
                newInvoiceData.Address = currentInvoice.Address;
            if (string.IsNullOrEmpty(newInvoiceData.Email))
                newInvoiceData.Email = currentInvoice.Email;

            var filter = Builders<InvoiceModel>.Filter.Eq("Id", newInvoiceData.Id);
            var update = Builders<InvoiceModel>.Update
                .Set(x => x.PaidStatus, newInvoiceData.PaidStatus)
                .Set(x => x.Price, newInvoiceData.Price)
                .Set(x => x.Description, newInvoiceData.Description)
                .Set(x => x.Address, newInvoiceData.Address)
                .Set(x => x.Email, newInvoiceData.Email);

            _invoices.UpdateOne(filter, update);
            return newInvoiceData;
        }

        public void ValidateInvoice(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", id);
            var update = Builders<InvoiceModel>.Update.Set(i => i.PaidStatus, true);

            _invoices.UpdateOne(filter, update);
            
            if (_invoices.UpdateOne(filter, update).MatchedCount == 0)
            {
                throw new KeyNotFoundException($"No invoice found with ID {id}.");
            }
            
            AuctionCoreLogger.Logger.Info($"Invoice validated with id: {id}");
        }
    }
}
