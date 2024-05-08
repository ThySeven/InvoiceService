using InvoiceService.Models;
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

        String InvoiceHtml = @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>Invoice</title>
<style>
  body {
    font-family: Arial, sans-serif;
    max-width: 800px;
    margin: 40px auto;
    padding: 20px;
    border: 1px solid #eee;
    border-radius: 10px;
  }
  .company-address, .invoice-details {
    margin-bottom: 20px;
  }
  .company-address h2, .invoice-details h2 {
    margin: 0 0 10px 0;
  }
  table {
    width: 100%;
    border-collapse: collapse;
  }
  table, th, td {
    border: 1px solid black;
  }
  th, td {
    padding: 10px;
    text-align: left;
  }
  .signature {
    margin-top: 40px;
  }
  .signature img {
    width: 150px;
  }
  .total-amount {
    text-align: right;
    margin-top: 20px;
  }
</style>
</head>
<body>

<div class=""company-address"">
  <h2>Grøn & Olsen</h2>
  <p>Sønderhøj 30, 8260 Viby J</p>
  <p>Phone: 88 88 88 88</p>
  <p>Email: grønogolsen@gmail.com</p>
</div>

<div class=""invoice-details"">
  <h2>Invoice #12345</h2>
  <p>Invoice Date: 2024-05-07</p>
  <p>Due Date: 2024-05-09</p>
</div>

<table>
  <thead>
    <tr>
      <th>Description</th>
      <th>Quantity</th>
      <th>Price</th>
      <th>Total</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Havetraktor</td>
      <td>2</td>
      <td>$100.00</td>
      <td>$200.00</td>
    </tr>
    <tr>
      <td>Bagger 293</td>
      <td>5</td>
      <td>$20.00</td>
      <td>$100.00</td>
    </tr>
    <tr>
      <td colspan=""3"" style=""text-align:right;""><strong>Grand Total</strong></td>
      <td><strong>$300.00</strong></td>
    </tr>
  </tbody>
</table>

<div class=""total-amount"">
  <h2>Total Due: $300.00</h2>
</div>

<div class=""signature"">
  <p>Authorized Signature</p>
  <img src=""https://scontent.fcph5-1.fna.fbcdn.net/v/t39.30808-6/441856076_122119247390268251_6872370368606673835_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=5f2048&_nc_ohc=kumg7n8kOKsQ7kNvgE--SRI&_nc_ht=scontent.fcph5-1.fna&oh=00_AfAofBAZoBPMHwwfex41mYJO0_s_qnRfTl91buhLlmz3MQ&oe=664113D0"" alt=""Signature"">
  <p>Kell Olsen, CEO</p>
</div>

</body>
</html>";

        AutoMail mail = new AutoMail()
        {
            DateTime = DateTime.Now,
            SenderMail = "gronogolsen@gmail.com",
            Header = "GrønOgOlsen Invoice - Betal!"
        };

        public InvoiceRepository()
        {
            queue = new RabbitMQueue();
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("AuctionCoreServices");
            _invoices = database.GetCollection<InvoiceModel>("Invoices");
            mail = new AutoMail()
            {
                DateTime = DateTime.Now,
                SenderMail = "gronogolsen@gmail.com",
                Header = "GrønOgOlsen Invoice - Betal!",
                Content = InvoiceHtml
            };
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
                Model = invoice,
                ReceiverMail = invoice.Email,
                DateTime = DateTime.Now,
                SenderMail = mail.SenderMail,
                Content = mail.Content,
                Header = mail.Header + DateTime.Now.Ticks
            });
        }

        public string CreatePaymentLink(PaymentModel payment)
        {
            //Seends Payment link to payment provider

            //Dummy integration with PayPal
            try
            {
                new HttpClient().PostAsJsonAsync("https://thisisyourdummypaymentlinknotintegratedtopaypal.yet/payment/submit", payment);
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
            try
            {
                AutoMail mail = new AutoMail()
                {
                    Model = invoice,
                    ReceiverMail = invoice.Email,
                    DateTime = DateTime.Now,
                    SenderMail = this.mail.SenderMail,
                    Content = this.mail.Content,
                    Header = this.mail.Header + DateTime.Now.Ticks
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
                new HttpClient().PostAsJsonAsync("https://thisisyourdummyparcelnotintegratedtogls.yet/shipment/submit", parcel);
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
            var filter = Builders<InvoiceModel>.Filter.Eq("Id", newInvoiceData.Id);
            var update = Builders<InvoiceModel>.Update
                            .Set(x => x.Price, newInvoiceData.Price); // Example of updating the amount
                                                                       // Add other properties to update as required
                            
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
